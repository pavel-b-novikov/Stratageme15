using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class UsingDirectiveSyntaxReactor : BasicReactorBase<UsingDirectiveSyntax>
    {
        protected override void HandleNode(UsingDirectiveSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            // todo handle nested usings
            result.Strategy = TranslationStrategy.DontTraverseChildren;
            string nm = node.Name.ToString();
            context.Usings.Add(nm);
        }
    }
}