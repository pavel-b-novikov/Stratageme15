using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class ArrayCreationExpressionSyntaxReactor :
        ExpressionReactorBase<ArrayCreationExpressionSyntax, ArrayCreationExpression>
    {
        public override ArrayCreationExpression TranslateNodeInner(ArrayCreationExpressionSyntax node,
                                                                   TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            result.PrepareForManualPush(context);
            if (node.Initializer != null)
            {
                context.TranslationStack.Push(node.Initializer);
            }
            return new ArrayCreationExpression();
        }
    }
}