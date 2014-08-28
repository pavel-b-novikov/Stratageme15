using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Binary;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Builders;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class MethodDeclarationSyntaxReactor : ReactorBase<MethodDeclarationSyntax>
    {
        protected override void HandleNode(MethodDeclarationSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            context.CurrentClassContext.PushFunction(node, node.Identifier.ValueText);
            context.PushTranslated(context.CurrentClassContext.CurrentFunction.Function);
            result.PrepareForManualPush(context);
            context.TranslationStack.Push(node.Body);
            context.TranslationStack.Push(node.ParameterList);
        }

        public override void OnAfterChildTraversal(TranslationContext context, MethodDeclarationSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            FunctionDefExpression fn = context.CurrentClassContext.CurrentFunction.Function;
            string name = fn.Name.Identifier;
            fn.Name = null;

            context.CurrentClassContext.PopFunction();
            context.PopTranslated();

            AssignmentBinaryExpression abe = null;
            var a = context.JavascriptCurrentTypeName()
                    .MemberAccess();
            if (!originalNode.Modifiers.Any(SyntaxKind.StaticKeyword))
            {
                a = a.Field("prototype");
            }
            a = a.Field(name);
            var b = a.Build();
            abe = b.Assignment(fn);

            context.TranslatedNode.CollectSymbol(abe);
        }
    }
}