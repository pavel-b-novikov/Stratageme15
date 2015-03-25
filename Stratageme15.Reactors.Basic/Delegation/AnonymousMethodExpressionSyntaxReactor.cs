using System;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Delegation
{
    class AnonymousMethodExpressionSyntaxReactor : ExpressionReactorBase<AnonymousMethodExpressionSyntax,Expression>
    {
        public override Expression TranslateNodeInner(AnonymousMethodExpressionSyntax node, TranslationContextWrapper context, TranslationResult result)
        {
            return null;
        }
    }
}
