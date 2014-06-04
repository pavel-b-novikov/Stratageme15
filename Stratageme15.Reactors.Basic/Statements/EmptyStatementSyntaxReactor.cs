using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements
{
    public class EmptyStatementSyntaxReactor : ReactorBase<EmptyStatementSyntax>
    {
        protected override void HandleNode(EmptyStatementSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.DontTraverseChildren;
            context.TranslatedNode.CollectSymbol(new EmptyStatement());
        }
    }
}
