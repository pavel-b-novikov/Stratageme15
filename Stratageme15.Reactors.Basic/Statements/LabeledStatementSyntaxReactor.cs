using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

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