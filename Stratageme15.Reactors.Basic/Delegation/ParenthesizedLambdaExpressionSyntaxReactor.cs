using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Stratageme15.Reactors.Basic.Delegation
{
    class ParenthesizedLambdaExpressionSyntaxReactor : ExpressionReactorBase<ParenthesizedLambdaExpressionSyntax,Expression>
    {
        public override Expression TranslateNodeInner(ParenthesizedLambdaExpressionSyntax node, TranslationContext context, TranslationResult result)
        {
            return null;
        }
    }
}
