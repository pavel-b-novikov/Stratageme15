﻿using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class ElementAccessExpressionSyntaxReactor : ExpressionReactorBase<ElementAccessExpressionSyntax,IndexerExpression>
    {
        public override IndexerExpression TranslateNodeInner(ElementAccessExpressionSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            return new IndexerExpression();
        }
    }
}
