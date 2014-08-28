using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Expressions
{
    internal class PostfixUnaryExpressionSyntaxReactor :
        ExpressionReactorBase<PostfixUnaryExpressionSyntax, PostfixIncrementDecrementExpression>
    {
        public override PostfixIncrementDecrementExpression TranslateNodeInner(PostfixUnaryExpressionSyntax node,
                                                                               TranslationContext context,
                                                                               TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            var res = new PostfixIncrementDecrementExpression();
            //todo get/set
            IndrementDecrementOperator opr = node.OperatorToken.CSharpKind().ConvertIndrementDecrement();
            res.CollectOperator(opr);
            return res;
        }
    }
}