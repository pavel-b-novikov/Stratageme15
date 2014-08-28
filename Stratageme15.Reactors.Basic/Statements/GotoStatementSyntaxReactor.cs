using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Logging;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements
{
    public class GotoStatementSyntaxReactor : ReactorBase<GotoStatementSyntax>
    {
        protected override void HandleNode(GotoStatementSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            context.Error("Goto in javascript is not supported. Re-organize your code to eliminate goto.");
            //todo add goto support for dummies
        }
    }
}