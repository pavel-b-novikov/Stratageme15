using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class ElementAccessExpressionSyntaxReactor :
        ExpressionReactorBase<ElementAccessExpressionSyntax, IndexerExpression>
    {
        public override IndexerExpression TranslateNodeInner(ElementAccessExpressionSyntax node,
                                                             TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            return new IndexerExpression();
        }
    }
}