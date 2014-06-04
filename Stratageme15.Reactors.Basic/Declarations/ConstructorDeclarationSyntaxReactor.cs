using Roslyn.Compilers.CSharp;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class ConstructorDeclarationSyntaxReactor : ReactorBase<ConstructorDeclarationSyntax>
    {
        protected override void HandleNode(ConstructorDeclarationSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            var typeName = context.JavascriptCurrentTypeName();
            context.CurrentClassContext.PushFunction(node,typeName);
            context.CurrentClassContext.CreateConstructor(context.CurrentClassContext.CurrentFunction.Function);
            context.TranslatedNode.CollectSymbol(context.CurrentClassContext.Constructor);

            context.PushTranslated(context.CurrentClassContext.CurrentFunction.Function);
        }

        public override void OnAfterChildTraversal(TranslationContext context, ConstructorDeclarationSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.CurrentClassContext.PopFunction();
            context.PopTranslated();
            
        }
    }
}
