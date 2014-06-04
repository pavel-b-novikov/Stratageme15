using System.Linq;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.Switch
{
    class SwitchSectionSyntaxReactor : ReactorBase<SwitchSectionSyntax>
    {
        protected override void HandleNode(SwitchSectionSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            result.PrepareForManualPush(context);
            foreach (var source in node.Labels.Reverse())
            {
                context.TranslationStack.Push(source);
            }
        }
    }
}
