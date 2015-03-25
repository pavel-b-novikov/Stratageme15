using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Delegation
{
    class ParenthesizedLambdaExpressionSyntaxReactor : ExpressionReactorBase<ParenthesizedLambdaExpressionSyntax,Expression>
    {
        public override Expression TranslateNodeInner(ParenthesizedLambdaExpressionSyntax node, TranslationContextWrapper context, TranslationResult result)
        {
            return null;
        }
    }
}
