using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Builders;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Declarations
{
    class ParameterListSyntaxReactor : ReactorBase<ParameterListSyntax>, IStandaloneReactor<ParameterListSyntax>
    {
        protected override void HandleNode(ParameterListSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.DontTraverseChildren;
            context.TranslatedNode.CollectSymbol(TranslateNode(node,context));
        }

        public SyntaxTreeNodeBase TranslateNode(SyntaxNode nd, TranslationContext context)
        {
            ParameterListSyntax node = (ParameterListSyntax) nd;
            FormalParametersList fpl = new FormalParametersList();

            foreach (var parameterSyntax in node.Parameters)
            {
                var parameterName = parameterSyntax.Identifier.ToString();
                fpl.CollectSymbol(parameterName.Ident());

                context.CurrentClassContext.CurrentFunction.LocalVariables.PushMethodParameter(parameterName, TypeInferer.GetTypeFromContext(parameterSyntax.Type, context));
            }
            return fpl;
        }
    }
}
