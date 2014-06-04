using System;
using System.Collections.Generic;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Transaltion.TranslationContexts
{
    public class ClassTranslationContext
    {
        private readonly Stack<FunctionTranslationContext> _functionsStack = new Stack<FunctionTranslationContext>();

        public ClassTranslationContext(ClassDeclarationSyntax originalNode,Type type)
        {
            OriginalNode = originalNode;
            Type = type;
        }

        public ClassDeclarationSyntax OriginalNode { get; private set; }
        public Type Type { get; set; }
        public ClassTranslationContext Parent { get; set; }
        public FunctionDefExpression Constructor { get; set; }


        public FunctionTranslationContext CurrentFunction
        {
            get { return _functionsStack.Peek(); }
        }

        public void PushFunction(SyntaxNode originalNode,string name = null)
        {
            _functionsStack.Push(new FunctionTranslationContext(originalNode,name));
        }

        public void PopFunction()
        {
            _functionsStack.Pop();
        }
    }
}