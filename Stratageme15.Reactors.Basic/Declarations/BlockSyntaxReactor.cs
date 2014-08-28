using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class BlockSyntaxReactor : ReactorBase<BlockSyntax>
    {
        protected override void HandleNode(BlockSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            context.CurrentClassContext.CurrentFunction.PushBlock();

            context.PushTranslated(context.CurrentClassContext.CurrentFunction.CurrentBlock);
        }

        public override void OnAfterChildTraversal(TranslationContext context, BlockSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.CurrentClassContext.CurrentFunction.PopBlock();
            context.PopTranslated();
        }
    }
}