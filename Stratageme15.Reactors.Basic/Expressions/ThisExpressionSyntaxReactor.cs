using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals.KeywordLiterals;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class ThisExpressionSyntaxReactor : ExpressionReactorBase<ThisExpressionSyntax, ThisKeywordLiteralExpression>
    {
        public override ThisKeywordLiteralExpression TranslateNodeInner(ThisExpressionSyntax node,
                                                                        TranslationContextWrapper context,
                                                                        TranslationResult res)
        {
            return new ThisKeywordLiteralExpression();
        }
    }
}