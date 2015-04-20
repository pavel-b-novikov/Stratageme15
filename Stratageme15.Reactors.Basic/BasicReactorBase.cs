using Microsoft.CodeAnalysis;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic
{
    /// <summary>
    /// Reactor base for basic reactors
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public abstract class BasicReactorBase<TNode> : ReactorBase<TNode> where TNode : SyntaxNode
    {
        protected override sealed void HandleNode(TNode node, TranslationContext context, TranslationResult result)
        {
            HandleNode(node,new TranslationContextWrapper(context), result );
        }

        public override sealed void OnAfterChildTraversal(TranslationContext context, TNode originalNode)
        {
            OnAfterChildTraversal(new TranslationContextWrapper(context), originalNode );
        }

        protected abstract void HandleNode(TNode node, TranslationContextWrapper context, TranslationResult result);

        public virtual void OnAfterChildTraversal(TranslationContextWrapper context, TNode originalNode)
        {
        }

    }
}
