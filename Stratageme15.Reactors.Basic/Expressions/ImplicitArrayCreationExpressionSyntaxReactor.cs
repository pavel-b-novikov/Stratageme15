using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class ImplicitArrayCreationExpressionSyntaxReactor :
        ExpressionReactorBase<ImplicitArrayCreationExpressionSyntax, ArrayCreationExpression>
    {
        public override ArrayCreationExpression TranslateNodeInner(ImplicitArrayCreationExpressionSyntax node,
                                                                   TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            return new ArrayCreationExpression();
        }
    }
}