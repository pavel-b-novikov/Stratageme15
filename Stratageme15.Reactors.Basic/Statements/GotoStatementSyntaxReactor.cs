using System;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements
{
    public class GotoStatementSyntaxReactor : ReactorBase<GotoStatementSyntax>
    {
        protected override void HandleNode(GotoStatementSyntax node, TranslationContext context, TranslationResult result)
        {
            throw new Exception("Goto in javascript is not supported. Re-organize your code to eliminate goto.");
            //todo add goto support for dummies
        }
    }
}
