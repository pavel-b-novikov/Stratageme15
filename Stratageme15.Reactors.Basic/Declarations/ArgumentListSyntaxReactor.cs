using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.TranslationContexts;
using Stratageme15.Reactors.Basic.Expressions;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class ArgumentListSyntaxReactor : ExpressionReactorBase<ArgumentListSyntax, FactParameterList>
    {
        public override FactParameterList TranslateNodeInner(ArgumentListSyntax node, TranslationContext context,
                                                             TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            return new FactParameterList();
        }
    }
}