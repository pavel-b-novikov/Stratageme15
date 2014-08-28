using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements
{
    internal class ContinueStatementSyntaxReactor : ReactorBase<ContinueStatementSyntax>
    {
        protected override void HandleNode(ContinueStatementSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.DontTraverseChildren;
            context.TranslatedNode.CollectSymbol(new ContinueStatement());
        }
    }
}