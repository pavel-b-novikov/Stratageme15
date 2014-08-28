using System;
using Microsoft.CodeAnalysis;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Core.Transaltion.Reactors
{
    public abstract class ReactorBase<TNode> : IReactor where TNode : SyntaxNode
    {
        #region IReactor Members

        public Type ReactedNodeType
        {
            get { return typeof (TNode); }
        }

        public TranslationResult OnNode(TranslationContext context)
        {
            var tr = new TranslationResult(TranslationStrategy.TraverseChildren);
            HandleNode((TNode) context.SourceNode, context, tr);
            return tr;
        }

        public void OnPromise(TranslationContext context, SyntaxNode originalNode)
        {
            OnAfterChildTraversal(context, (TNode) originalNode);
        }

        #endregion

        protected abstract void HandleNode(TNode node, TranslationContext context, TranslationResult result);

        public virtual void OnAfterChildTraversal(TranslationContext context, TNode originalNode)
        {
        }
    }
}