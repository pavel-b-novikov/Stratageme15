using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class ParenthesizedExpressionSyntaxReactor : ExpressionReactorBase<ParenthesizedExpressionSyntax,ParenthesisExpression>
    {
        public override ParenthesisExpression TranslateNodeInner(ParenthesizedExpressionSyntax node, TranslationContext context,TranslationResult res)
        {
            res.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            ParenthesisExpression result = new ParenthesisExpression();
            return result;
        }
    }
}
