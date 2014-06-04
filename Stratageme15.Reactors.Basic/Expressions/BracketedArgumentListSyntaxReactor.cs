using System.Linq;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class BracketedArgumentListSyntaxReactor : ExpressionReactorBase<BracketedArgumentListSyntax,IndexExpression>
    {
        public override IndexExpression TranslateNodeInner(BracketedArgumentListSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            result.PrepareForManualPush(context);
            foreach (var argumentSyntax in node.Arguments.Reverse())
            {
                context.TranslationStack.Push(argumentSyntax.Expression);
            }
            return new IndexExpression();
        }
    }
}
