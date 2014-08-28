using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    internal class ObjectCreationExpressionSyntaxReactor :
        ExpressionReactorBase<ObjectCreationExpressionSyntax, NewInvokationExpression>
    {
        public override NewInvokationExpression TranslateNodeInner(ObjectCreationExpressionSyntax node,
                                                                   TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            var nw = new NewInvokationExpression();
            return nw;
        }
    }
}