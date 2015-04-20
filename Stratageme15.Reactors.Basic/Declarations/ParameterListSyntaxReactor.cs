using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Declarations
{
    internal class ParameterListSyntaxReactor : BasicReactorBase<ParameterListSyntax>
    {
        protected override void HandleNode(ParameterListSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.DontTraverseChildren;
            context.Context.TargetNode.CollectSymbol(TranslateNode(node, context));
        }

        public static SyntaxTreeNodeBase TranslateNode(SyntaxNode nd, TranslationContextWrapper context)
        {
            var node = (ParameterListSyntax) nd;
            var fpl = new FormalParametersList();

            foreach (ParameterSyntax parameterSyntax in node.Parameters)
            {
                string parameterName = parameterSyntax.Identifier.ToString();
                fpl.CollectSymbol(parameterName.ToIdent());

                context.CurrentClassContext
                    .CurrentFunction
                    .LocalVariables
                    .PushMethodParameter(parameterName,
                                        context.Context.SemanticModel.GetTypeInfo(parameterSyntax)
                                        );
            }
            return fpl;
        }
    }
}