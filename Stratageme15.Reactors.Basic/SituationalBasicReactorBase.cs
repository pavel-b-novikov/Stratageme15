using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic
{
    public abstract class SituationalBasicReactorBase<TNode> : BasicReactorBase<TNode>, ISituationReactor where TNode : SyntaxNode
    {
        public bool IsAcceptable(TranslationContext context)
        {
            return IsAcceptable(new TranslationContextWrapper(context));
        }

        protected abstract bool IsAcceptable(TranslationContextWrapper wrapper);
    }
}
