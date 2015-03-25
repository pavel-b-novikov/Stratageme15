using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class BlockSyntaxReactor : BasicReactorBase<BlockSyntax>
    {
        protected override void HandleNode(BlockSyntax node, TranslationContextWrapper context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            context.CurrentClassContext.CurrentFunction.PushBlock();

            context.Context.PushTranslated(context.CurrentClassContext.CurrentFunction.CurrentBlock);
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, BlockSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.CurrentClassContext.CurrentFunction.PopBlock();
            context.Context.PopTranslated();
        }
    }
}