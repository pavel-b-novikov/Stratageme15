using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.TranslationContexts;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class MemberAccessExpressionSyntaxReactor :
        ExpressionReactorBase<MemberAccessExpressionSyntax, FieldAccessExpression>
    {
        public override FieldAccessExpression TranslateNodeInner(MemberAccessExpressionSyntax node,
                                                                 TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            string memberName = node.Name.Identifier.ValueText;
            Type accesseeType = TypeInferer.InferTypeFromExpression(node.Expression, context);
            if (accesseeType.IsProperty(memberName))
            {
                result.PrepareForManualPush(context);
                string getter = string.Format("get{0}", memberName);
                MemberAccessExpressionSyntax modified = node.WithName(SyntaxFactory.IdentifierName(getter));
                InvocationExpressionSyntax invokation = SyntaxFactory.InvocationExpression(modified);
                context.TranslationStack.Push(invokation);
                return null;
            }
            var fae = new FieldAccessExpression();
            return fae;
        }
    }
}