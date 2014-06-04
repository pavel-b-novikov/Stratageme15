using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.TranslationContexts;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Expressions
{
    class PostfixUnaryExpressionSyntaxReactor : ExpressionReactorBase<PostfixUnaryExpressionSyntax, PostfixIncrementDecrementExpression>
    {
        public override PostfixIncrementDecrementExpression TranslateNodeInner(PostfixUnaryExpressionSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            var res = new PostfixIncrementDecrementExpression();
            var opr = node.OperatorToken.Kind.ConvertIndrementDecrement();
            res.CollectOperator(opr);
            return res;
        }
    }
}
