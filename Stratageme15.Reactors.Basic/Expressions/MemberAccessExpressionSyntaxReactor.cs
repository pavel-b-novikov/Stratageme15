using System;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Builders;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class MemberAccessExpressionSyntaxReactor : ExpressionReactorBase<MemberAccessExpressionSyntax,FieldAccessExpression>
    {
        public override FieldAccessExpression TranslateNodeInner(MemberAccessExpressionSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            var fae = new FieldAccessExpression();
            //fae.Member = node.Name.Identifier.ValueText.Ident();
            return fae;
        }
    }
}
