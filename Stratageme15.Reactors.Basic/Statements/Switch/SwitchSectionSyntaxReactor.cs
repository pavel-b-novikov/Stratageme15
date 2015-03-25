using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements.Switch
{
    internal class SwitchSectionSyntaxReactor : BasicReactorBase<SwitchSectionSyntax>
    {
        protected override void HandleNode(SwitchSectionSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            result.PrepareForManualPush(context.Context);
            foreach (SwitchLabelSyntax source in node.Labels.Reverse())
            {
                context.Context.TranslationStack.Push(source);
            }
        }
    }
}