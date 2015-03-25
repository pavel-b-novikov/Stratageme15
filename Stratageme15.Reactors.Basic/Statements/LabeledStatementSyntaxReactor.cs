using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements
{
    public class LabeledStatementSyntaxReactor : BasicReactorBase<LabeledStatementSyntax>
    {
        protected override void HandleNode(LabeledStatementSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            throw new Exception("statement labels still not supported");
        }
    }
}