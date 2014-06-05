using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Builders;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class IdentifierNameSyntaxReactor : ExpressionReactorBase<IdentifierNameSyntax,IdentExpression>,ISituationReactor
    {
        public override IdentExpression TranslateNodeInner(IdentifierNameSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.DontTraverseChildren;
            if (context.TranslatedNode is NewInvokationExpression)
            {
                var type = TypeInferer.InferTypeFromExpression(node, context);
                if (type!=null)
                {
                    return type.JavascriptTypeName().Ident();
                }
            }
            return node.Identifier.ValueText.Ident();
            // todo usage of local variables
        }

        public bool IsAcceptable(TranslationContext context)
        {
            var acc= (
                    typeof (Expression).IsAssignableFrom(context.TranslatedNode.GetType())
                    || typeof(FactParameterList).IsAssignableFrom(context.TranslatedNode.GetType())
                    || typeof(IndexExpression).IsAssignableFrom(context.TranslatedNode.GetType())
                    || typeof(IStatement).IsAssignableFrom(context.TranslatedNode.GetType())
                )&&(
                    ((IdentifierNameSyntax)context.SourceNode).Identifier.ValueText!="var"
                )
                ;

            return acc;
        }
    }
}
