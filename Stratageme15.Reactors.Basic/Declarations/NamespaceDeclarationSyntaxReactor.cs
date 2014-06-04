using Roslyn.Compilers.CSharp;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Declarations
{
    class NamespaceDeclarationSyntaxReactor : ReactorBase<NamespaceDeclarationSyntax>
    {
        protected override void HandleNode(NamespaceDeclarationSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            context.Namespace = node.Name.ToString();
        }
    }
}
