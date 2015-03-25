using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Expressions
{
    internal class PrefixUnaryExpressionSyntaxReactor :
        ExpressionReactorBase<PrefixUnaryExpressionSyntax, UnaryExpression>
    {
        public override UnaryExpression TranslateNodeInner(PrefixUnaryExpressionSyntax node, TranslationContextWrapper context,
                                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            var uex = new UnaryExpression();
            uex.CollectOperator(node.OperatorToken.CSharpKind().ConvertUnary());
            return uex;
        }
    }
}