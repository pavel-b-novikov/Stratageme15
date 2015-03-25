using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class ArrayCreationExpressionSyntaxReactor :
        ExpressionReactorBase<ArrayCreationExpressionSyntax, ArrayCreationExpression>
    {
        public override ArrayCreationExpression TranslateNodeInner(ArrayCreationExpressionSyntax node,
                                                                   TranslationContextWrapper context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            result.PrepareForManualPush(context.Context);
            if (node.Initializer != null)
            {
                context.Context.TranslationStack.Push(node.Initializer);
            }
            return new ArrayCreationExpression();
        }
    }
}