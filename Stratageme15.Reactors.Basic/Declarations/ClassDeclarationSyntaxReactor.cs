using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Binary;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Builders;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class ClassDeclarationSyntaxReactor : ReactorBase<ClassDeclarationSyntax>
    {
        protected override void HandleNode(ClassDeclarationSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            if (node.Modifiers.Any(SyntaxKind.StaticKeyword)) throw new Exception("Static classes are not supported");
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;

            object className = node.Identifier.Value;
            string fullTypeName = string.Format("{0}.{1}", context.Namespace, className);
            Type type = context.Assemblies.GetType(fullTypeName);

            context.PushClass(new ClassTranslationContext(node, type, context));
            result.PrepareForManualPush(context);
            IEnumerable<MemberDeclarationSyntax> members =
                node.Members.OrderByDescending(c => c is ConstructorDeclarationSyntax).Reverse();

            foreach (MemberDeclarationSyntax memberDeclarationSyntax in members)
            {
                context.TranslationStack.Push(memberDeclarationSyntax);
            }

            if (!node.Members.Any(c => c is ConstructorDeclarationSyntax))
            {
                context.CurrentClassContext.CreateConstructor(type.JavascriptTypeName());
                context.TranslatedNode.CollectSymbol(context.CurrentClassContext.Constructor);
            }
        }

        public override void OnAfterChildTraversal(TranslationContext context, ClassDeclarationSyntax originalNode)
        {
            AssignmentBinaryExpression abe =
                context.JavascriptCurrentTypeName()
                    .MemberAccess()
                    .Field("prototype")
                    .Field("constructor")
                    .Build()
                    .Assignment(context.JavascriptCurrentTypeName());

            context.TranslatedNode.CollectSymbol(abe);
            context.TranslatedNode.EmptyColon();
            context.PopClass();
        }
    }
}