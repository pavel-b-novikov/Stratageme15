using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.Switch
{
    internal class SwitchSectionSyntaxReactor : ReactorBase<SwitchSectionSyntax>
    {
        protected override void HandleNode(SwitchSectionSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            result.PrepareForManualPush(context);
            foreach (SwitchLabelSyntax source in node.Labels.Reverse())
            {
                context.TranslationStack.Push(source);
            }
        }
    }
}