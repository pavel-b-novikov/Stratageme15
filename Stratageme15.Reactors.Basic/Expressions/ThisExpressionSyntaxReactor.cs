using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals.KeywordLiterals;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class ThisExpressionSyntaxReactor : ExpressionReactorBase<ThisExpressionSyntax,ThisKeywordLiteralExpression>
    {
        public override ThisKeywordLiteralExpression TranslateNodeInner(ThisExpressionSyntax node, TranslationContext context, TranslationResult res)
        {
            return new ThisKeywordLiteralExpression();
        }
    }
}
