using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Declarations
{
    internal class NamespaceDeclarationSyntaxReactor : ReactorBase<NamespaceDeclarationSyntax>
    {
        protected override void HandleNode(NamespaceDeclarationSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            context.Namespace = node.Name.ToString();
        }
    }
}