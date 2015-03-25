using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class InvocationExpressionSyntaxReactor : ExpressionReactorBase<InvocationExpressionSyntax, CallExpression>
    {
        public override CallExpression TranslateNodeInner(InvocationExpressionSyntax node, TranslationContextWrapper context,
                                                          TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            return new CallExpression();
        }
    }
}