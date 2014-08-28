using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Builders;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Declarations
{
    internal class ParameterListSyntaxReactor : ReactorBase<ParameterListSyntax>
    {
        protected override void HandleNode(ParameterListSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.DontTraverseChildren;
            context.TranslatedNode.CollectSymbol(TranslateNode(node, context));
        }

        public SyntaxTreeNodeBase TranslateNode(SyntaxNode nd, TranslationContext context)
        {
            var node = (ParameterListSyntax) nd;
            var fpl = new FormalParametersList();

            foreach (ParameterSyntax parameterSyntax in node.Parameters)
            {
                string parameterName = parameterSyntax.Identifier.ToString();
                fpl.CollectSymbol(parameterName.Ident());

                context.CurrentClassContext.CurrentFunction.LocalVariables.PushMethodParameter(parameterName,
                                                                                               TypeInferer.
                                                                                                   GetTypeFromContext(
                                                                                                       parameterSyntax.
                                                                                                           Type, context));
            }
            return fpl;
        }
    }
}