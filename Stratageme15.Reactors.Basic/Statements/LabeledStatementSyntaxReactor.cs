using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements
{
    public class LabeledStatementSyntaxReactor : ReactorBase<LabeledStatementSyntax>
    {
        protected override void HandleNode(LabeledStatementSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            throw new Exception("statement labels still not supported");
        }
    }
}