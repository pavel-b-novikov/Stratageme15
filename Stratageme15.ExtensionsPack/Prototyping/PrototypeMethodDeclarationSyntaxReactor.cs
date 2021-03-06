﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Binary;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.ExtensionsPack.Prototyping
{
    public class PrototypeMethodDeclarationSyntaxReactor : ReactorBase<MethodDeclarationSyntax>, ISituationReactor
    {
        protected override void HandleNode(MethodDeclarationSyntax node, TranslationContext context, TranslationResult result)
        {
            //if (!node.Modifiers.Any(SyntaxKind.StaticKeyword)) throw new Exception("Not allowed static method in prototype declaration");
            //result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            //context.CurrentClassContext.PushFunction(node, node.Identifier.ValueText);
            //context.PushTranslated(context.CurrentClassContext.CurrentFunction.Function);
        }

        public override void OnAfterChildTraversal(TranslationContext context, MethodDeclarationSyntax originalNode)
        {
            //base.OnAfterChildTraversal(context, originalNode);
            //var fn = context.CurrentClassContext.CurrentFunction.Function;
            //var name = fn.Name.Identifier;
            //fn.Name = null;

            //context.CurrentClassContext.PopFunction();
            //context.PopTranslated();

            //AssignmentBinaryExpression abe =
            //  context.CurrentClassContext.GetCustomVariable<PrototypeAttribute>(PrototypeAttribute.PrototypeAttrKey).PrototypeName
            //      .MemberAccess()
            //          .Field("prototype")
            //          .Field(name)
            //          .Build()
            //      .Assignment(fn);

            //context.TranslatedNode.CollectSymbol(abe);
        }

        public bool IsAcceptable(TranslationContext context)
        {
            //return context.CurrentClassContext.IsCustomVariableDefined(PrototypeAttribute.PrototypeAttrKey);
            return false;
        }
    }
}
