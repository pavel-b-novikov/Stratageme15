using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class ImplicitArrayCreationExpressionSyntaxReactor : ExpressionReactorBase<ImplicitArrayCreationExpressionSyntax,ArrayCreationExpression>
    {
        public override ArrayCreationExpression TranslateNodeInner(ImplicitArrayCreationExpressionSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            return new ArrayCreationExpression();
        }
    }
}
