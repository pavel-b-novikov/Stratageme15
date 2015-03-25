using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Reactors.Basic.Contexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class ClassDeclarationSyntaxReactor : BasicReactorBase<ClassDeclarationSyntax>
    {
        protected override void HandleNode(ClassDeclarationSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            if (node.Modifiers.Any(SyntaxKind.StaticKeyword)) throw new Exception("Static classes are not supported");
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;

            string fullName = node.FullQualifiedName();
            var type = context.Compilation.GetTypeByMetadataName(fullName);
            if (context.CurrentClassContext == null)
            {
                context.CurrentClassContext = new ClassTranslationContext(node, type, context);

                if (node.Members.Any(c => c is ConstructorDeclarationSyntax))
                {
                    var allConstructors = node.Members.Where(c => c is ConstructorDeclarationSyntax);
                    List<string> constructorParameters = new List<string>();

                }

                result.PrepareForManualPush(context.Context);
                IEnumerable<MemberDeclarationSyntax> members =
                    node.Members.OrderBy(
                    c => (c is ClassDeclarationSyntax) ? 0 
                        : (c is ConstructorDeclarationSyntax) ? 1 
                        : 2)
                    .Reverse();

                foreach (MemberDeclarationSyntax memberDeclarationSyntax in members)
                {
                    context.Context.TranslationStack.Push(memberDeclarationSyntax);
                }
            }else
            {
                var newContext = context.CurrentClassContext.Nest(node,type);
                context.CurrentClassContext = newContext;
            }
            RegisterPolymorphs(node,context);
        }

        private void RegisterPolymorphs(ClassDeclarationSyntax cDecl, TranslationContextWrapper context)
        {
            var methods = cDecl.Members.Where(c => c is MethodDeclarationSyntax).Cast<MethodDeclarationSyntax>();
            var polymorphs = methods
                .GroupBy(c => c.Identifier.ValueText)
                .Where(c => c.Count() > 1).Select(c => c.Key);
            foreach (var polymorph in polymorphs)
            {
                context.Polymorphism.RegisterPolymorphism(polymorph);
            }
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, ClassDeclarationSyntax originalNode)
        {
            if (context.CurrentClassContext.IsNested)
            {
                var oldParent = context.CurrentClassContext.NestEnd();
                context.CurrentClassContext = oldParent;
            }
            else
            {
                var emit = context.CurrentClassContext.EmitClassDeclaration();
                context.Context.TargetNode.CollectSymbol(emit);
                context.Context.TargetNode.EmptyColon();
                context.CurrentClassContext = null;
            }
        }
    }
}