using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class BracketedArgumentListSyntaxReactor :
        ExpressionReactorBase<BracketedArgumentListSyntax, IndexExpression>
    {
        public override IndexExpression TranslateNodeInner(BracketedArgumentListSyntax node, TranslationContextWrapper context,
                                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            result.PrepareForManualPush(context.Context);
            foreach (ArgumentSyntax argumentSyntax in node.Arguments.Reverse())
            {
                context.Context.TranslationStack.Push(argumentSyntax.Expression);
            }
            return new IndexExpression();
        }
    }
}