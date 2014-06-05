using System;
using System.Collections.Generic;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Transaltion.Builders;

namespace Stratageme15.Core.Transaltion.TranslationContexts
{
    /// <summary>
    /// Holds information for class being translated
    /// </summary>
    public class ClassTranslationContext
    {
        private readonly Stack<FunctionTranslationContext> _functionsStack = new Stack<FunctionTranslationContext>();

        public ClassTranslationContext(ClassDeclarationSyntax originalNode,Type type)
        {
            OriginalNode = originalNode;
            Type = type;
        }

        /// <summary>
        /// Original C# node with class description
        /// </summary>
        public ClassDeclarationSyntax OriginalNode { get; private set; }

        /// <summary>
        /// Translating class type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Parent class (only for nested classes)
        /// </summary>
        public ClassTranslationContext Parent { get; set; }

        /// <summary>
        /// Class constructor function
        /// </summary>
        public FunctionDefExpression Constructor { get; private set; }
        
        /// <summary>
        /// Code block for fields definitions and initialization
        /// </summary>
        public CodeBlock FieldsDefinitionBlock { get; private set; }

        public void CreateConstructor(string typeName)
        {
            var fn = new FunctionDefExpression() {Name = typeName.Ident()};
            var cf = new CodeBlock();
            fn.CollectSymbol(cf);
            FieldsDefinitionBlock = new CodeBlock();
            cf.CollectSymbol(FieldsDefinitionBlock);
            FieldsDefinitionBlock.Parent = cf;
            Constructor = fn;
        }

        private bool _outOfContext = false;
        /// <summary>
        /// Temporary disables access to function context due to going out of method translation
        /// </summary>
        public void OutOfContext()
        {
            _outOfContext = true;
        }

        /// <summary>
        /// Enables access to function context after OutOfContext call
        /// </summary>
        public void ReturnToContext()
        {
            _outOfContext = false;
        }
        public FunctionTranslationContext CurrentFunction
        {
            get { return _outOfContext?null: _functionsStack.Peek(); }
        }

        public void PushFunction(SyntaxNode originalNode,string name = null)
        {
            _functionsStack.Push(new FunctionTranslationContext(originalNode,name));
        }

        public void PushFunction(SyntaxNode originalNode,FunctionDefExpression existing)
        {
            _functionsStack.Push(new FunctionTranslationContext(originalNode,existing));
        }

        public void PopFunction()
        {
            _functionsStack.Pop();
        }
    }
}