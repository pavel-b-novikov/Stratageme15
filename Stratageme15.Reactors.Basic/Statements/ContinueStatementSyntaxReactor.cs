using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements
{
    class ContinueStatementSyntaxReactor : ReactorBase<ContinueStatementSyntax>
    {
        protected override void HandleNode(ContinueStatementSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.DontTraverseChildren;
            context.TranslatedNode.CollectSymbol(new ContinueStatement());
        }
    }
}
