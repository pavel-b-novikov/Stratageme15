using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class ConditionalExpressionSyntaxReactor :
        ExpressionReactorBase<ConditionalExpressionSyntax, TernaryStatement>
    {
        public override TernaryStatement TranslateNodeInner(ConditionalExpressionSyntax node, TranslationContext context,
                                                            TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            return new TernaryStatement();
        }
    }
}