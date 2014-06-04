using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.TranslationContexts;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Expressions
{
    class PrefixUnaryExpressionSyntaxReactor : ExpressionReactorBase<PrefixUnaryExpressionSyntax, UnaryExpression>
    {
        public override UnaryExpression TranslateNodeInner(PrefixUnaryExpressionSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            UnaryExpression uex = new UnaryExpression();
            uex.CollectOperator(node.OperatorToken.Kind.ConvertUnary());
            return uex;
        }
    }
}
