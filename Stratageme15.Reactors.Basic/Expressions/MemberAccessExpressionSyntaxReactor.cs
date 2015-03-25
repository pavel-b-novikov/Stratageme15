using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;
using TypeInfo = Microsoft.CodeAnalysis.TypeInfo;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class MemberAccessExpressionSyntaxReactor :
        ExpressionReactorBase<MemberAccessExpressionSyntax, FieldAccessExpression>
    {
        public override FieldAccessExpression TranslateNodeInner(MemberAccessExpressionSyntax node,
                                                                 TranslationContextWrapper context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            string memberName = node.Name.Identifier.ValueText;
            TypeInfo accesseeType = context.Context.SemanticModel.GetTypeInfo(node.Expression);
            if (accesseeType.ConvertedType.IsProperty(memberName))
            {
                result.PrepareForManualPush(context.Context);
                string getter = string.Format("get{0}", memberName);
                MemberAccessExpressionSyntax modified = node.WithName(SyntaxFactory.IdentifierName(getter));
                InvocationExpressionSyntax invokation = SyntaxFactory.InvocationExpression(modified);
                context.Context.TranslationStack.Push(invokation);
                return null;
            }
            var fae = new FieldAccessExpression();
            return fae;
        }
    }
}