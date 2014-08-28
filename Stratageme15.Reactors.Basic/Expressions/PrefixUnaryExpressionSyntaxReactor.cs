using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.TranslationContexts;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Expressions
{
    internal class PrefixUnaryExpressionSyntaxReactor :
        ExpressionReactorBase<PrefixUnaryExpressionSyntax, UnaryExpression>
    {
        public override UnaryExpression TranslateNodeInner(PrefixUnaryExpressionSyntax node, TranslationContext context,
                                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            var uex = new UnaryExpression();
            uex.CollectOperator(node.OperatorToken.CSharpKind().ConvertUnary());
            return uex;
        }
    }
}