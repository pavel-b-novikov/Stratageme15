using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class UsingDirectiveSyntaxReactor : ReactorBase<UsingDirectiveSyntax>
    {
        protected override void HandleNode(UsingDirectiveSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            // todo handle nested usings
            result.Strategy = TranslationStrategy.DontTraverseChildren;
            string nm = node.Name.ToString();
            context.Usings.Add(nm);
        }
    }
}