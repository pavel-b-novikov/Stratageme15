using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class ParenthesizedExpressionSyntaxReactor :
        ExpressionReactorBase<ParenthesizedExpressionSyntax, ParenthesisExpression>
    {
        public override ParenthesisExpression TranslateNodeInner(ParenthesizedExpressionSyntax node,
                                                                 TranslationContext context, TranslationResult res)
        {
            res.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            var result = new ParenthesisExpression();
            return result;
        }
    }
}