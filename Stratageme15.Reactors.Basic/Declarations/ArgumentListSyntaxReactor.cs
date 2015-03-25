using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Expressions;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class ArgumentListSyntaxReactor : ExpressionReactorBase<ArgumentListSyntax, FactParameterList>
    {
        public override FactParameterList TranslateNodeInner(ArgumentListSyntax node, TranslationContextWrapper context,
                                                             TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            return new FactParameterList();
        }
    }
}