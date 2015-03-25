using System;
using Microsoft.CodeAnalysis;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Core.Translation.Reactors
{
    public interface IReactor
    {
        Type ReactedNodeType { get; }
        TranslationResult OnNode(TranslationContext context);
        void OnPromise(TranslationContext context, SyntaxNode originalNode);
    }
}