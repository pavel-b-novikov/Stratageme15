using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.ExtensionsPack.Prototyping
{
    class PrototypeClassDeclarationSyntaxReactor : ReactorBase<ClassDeclarationSyntax>
    {
        protected override void HandleNode(ClassDeclarationSyntax node, TranslationContext context, TranslationResult result)
        {
            //if (!node.Modifiers.Any(SyntaxKind.StaticKeyword))
            //{
            //    result.FallDownToNextReactor();
            //    return;
            //}

            //var className = node.Identifier.Value;
            //var fullTypeName = string.Format("{0}.{1}", context.Namespace, className);
            //var type = context.Assemblies.GetType(fullTypeName);
            //var attr = type.GetCustomAttributes(typeof (PrototypeAttribute), true);
            //if (!attr.Any())
            //{
            //    result.FallDownToNextReactor();
            //    return;
            //}
            //var proto = (PrototypeAttribute) attr.First();
            //context.PushClass(new ClassTranslationContext(node, type,context));
            //result.PrepareForManualPush(context);
            //context.CurrentClassContext[PrototypeAttribute.PrototypeAttrKey] = proto;

            //var methods =
            //    node.Members.Where(
            //        m =>
            //        m is MethodDeclarationSyntax &&
            //        ((MethodDeclarationSyntax) m).Modifiers.Any(SyntaxKind.StaticKeyword));
            //foreach (var meth in methods)
            //{
            //    context.TranslationStack.Push(meth);
            //}

        }

        public override void OnAfterChildTraversal(TranslationContext context, ClassDeclarationSyntax originalNode)
        {
            //context.TranslatedNode.EmptyColon();
            //context.PopClass();
        }
    }
}
