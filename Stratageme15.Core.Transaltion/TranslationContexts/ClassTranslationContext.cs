using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals.KeywordLiterals;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Transaltion.Builders;

namespace Stratageme15.Core.Transaltion.TranslationContexts
{
    /// <summary>
    /// Holds information for class being translated
    /// </summary>
    public class ClassTranslationContext
    {
        private readonly TranslationContext _context;
        private readonly Dictionary<string, object> _customContextVars = new Dictionary<string, object>();
        private readonly Stack<FunctionTranslationContext> _functionsStack = new Stack<FunctionTranslationContext>();
        private bool _outOfContext;

        public ClassTranslationContext(ClassDeclarationSyntax originalNode, Type type, TranslationContext context)
        {
            OriginalNode = originalNode;
            Type = type;
            _context = context;
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

        /// <summary>
        /// Gets current function translation context
        /// </summary>
        public FunctionTranslationContext CurrentFunction
        {
            get { return _outOfContext ? null : _functionsStack.Peek(); }
        }

        public object this[string s]
        {
            get { return _customContextVars[s]; }
            set { _customContextVars[s] = value; }
        }

        public void CreateConstructor(string typeName)
        {
            var fn = new FunctionDefExpression {Name = typeName.Ident()};
            var cf = new CodeBlock();
            fn.CollectSymbol(cf);
            FieldsDefinitionBlock = new CodeBlock();
            cf.CollectSymbol(FieldsDefinitionBlock);
            FieldsDefinitionBlock.Parent = cf;
            Constructor = fn;
        }

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

        public void PushFunction(SyntaxNode originalNode, string name = null)
        {
            _functionsStack.Push(new FunctionTranslationContext(_context, originalNode, name));
        }

        public void PushFunction(SyntaxNode originalNode, FunctionDefExpression existing)
        {
            _functionsStack.Push(new FunctionTranslationContext(_context, originalNode, existing));
        }

        public void PopFunction()
        {
            _functionsStack.Pop();
        }

        public T GetCustomVariable<T>(string s)
        {
            return (T) _customContextVars[s];
        }

        public bool IsCustomVariableDefined(string s)
        {
            return _customContextVars.ContainsKey(s);
        }

        public void DropCustomVariable(string s)
        {
            _customContextVars.Remove(s);
        }
    }
}