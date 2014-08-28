using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements
{
    public class EmptyStatementSyntaxReactor : ReactorBase<EmptyStatementSyntax>
    {
        protected override void HandleNode(EmptyStatementSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.DontTraverseChildren;
            context.TranslatedNode.CollectSymbol(new EmptyStatement());
        }
    }
}