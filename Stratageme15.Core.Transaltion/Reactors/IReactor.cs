using System;
using System.Collections.Generic;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Core.Transaltion.Reactors
{
    public interface IReactor
    {
        Type ReactedNodeType { get; }
        TranslationResult OnNode(TranslationContext context);
        void OnPromise(TranslationContext context,SyntaxNode originalNode);
    }
}
