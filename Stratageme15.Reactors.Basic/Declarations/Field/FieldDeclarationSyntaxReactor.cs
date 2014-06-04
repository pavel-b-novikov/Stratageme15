using Roslyn.Compilers.CSharp;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Declarations.Field
{
    public class FieldDeclarationSyntaxReactor : ReactorBase<FieldDeclarationSyntax>
    {

        protected override void HandleNode(FieldDeclarationSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            context.SetTranslated(context.CurrentClassContext.FieldsDefinitionBlock);
            
        }

        public override void OnAfterChildTraversal(TranslationContext context, FieldDeclarationSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.RestoreTranslated();
        }
    }
}
