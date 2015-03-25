using System;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Translation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Delegation
{
    class SimpleLambdaExpressionSyntaxReactor : BasicReactorBase<SimpleLambdaExpressionSyntax>
    {
        protected override void HandleNode(SimpleLambdaExpressionSyntax node, TranslationContextWrapper context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            result.PrepareForManualPush(context.Context);

            FunctionDefExpression fde = new FunctionDefExpression();
            fde.Parameters = new FormalParametersList();
            fde.Parameters.CollectSymbol(node.Parameter.Identifier.ValueText.ToIdent());
            Type argType = null;
            if (node.Parameter.Type!=null)
            {
                //argType = TypeInferer.GetTypeFromContext(node.Parameter.Type, context);
            }else
            {
                
            }

            //context.CurrentClassContext.EnterClosureContext();

            context.Context.PushTranslated(fde);
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, SimpleLambdaExpressionSyntax originalNode)
        {
            context.Context.PopTranslated();
            //context.ExitClosureContext();
        }
    }
}
