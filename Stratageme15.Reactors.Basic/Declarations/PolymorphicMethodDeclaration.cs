using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class PolymorphicMethodDeclaration : SituationalBasicReactorBase<MethodDeclarationSyntax>
    {
        protected override void HandleNode(MethodDeclarationSyntax node, TranslationContextWrapper context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            context.CurrentClassContext.Polymorphism.PushPolymorphism(node, context.Context);
            result.PrepareForManualPush(context.Context);
            context.Context.TranslationStack.Push(node.Body);
            // todo need to push function here
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, MethodDeclarationSyntax originalNode)
        {
            context.CurrentClassContext.Polymorphism.PopPolymorphism(context.Context);
        }

        protected override bool IsAcceptable(TranslationContextWrapper wrapper)
        {
            MethodDeclarationSyntax mdc = (MethodDeclarationSyntax)wrapper.Context.SourceNode;
            return wrapper.CurrentClassContext.Polymorphism.LookupPolymorohism(mdc);
        }
    }
}
