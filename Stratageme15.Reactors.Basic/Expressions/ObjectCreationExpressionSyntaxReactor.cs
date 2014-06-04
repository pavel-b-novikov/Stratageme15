using System;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    class ObjectCreationExpressionSyntaxReactor : ExpressionReactorBase<ObjectCreationExpressionSyntax,NewInvokationExpression>
    {
        public override NewInvokationExpression TranslateNodeInner(ObjectCreationExpressionSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            NewInvokationExpression nw = new NewInvokationExpression();
            return nw;

        }
    }
}
