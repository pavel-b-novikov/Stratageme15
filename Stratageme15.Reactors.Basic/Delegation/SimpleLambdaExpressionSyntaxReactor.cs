using System;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Builders;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;
using Stratageme15.Reactors.Basic.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Delegation
{
    class SimpleLambdaExpressionSyntaxReactor : ReactorBase<SimpleLambdaExpressionSyntax>
    {
        protected override void HandleNode(SimpleLambdaExpressionSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            result.PrepareForManualPush(context);

            FunctionDefExpression fde = new FunctionDefExpression();
            fde.Parameters = new FormalParametersList();
            fde.Parameters.CollectSymbol(node.Parameter.Identifier.ValueText.Ident());
            Type argType = null;
            if (node.Parameter.Type!=null)
            {
                argType = TypeInferer.GetTypeFromContext(node.Parameter.Type, context);
            }else
            {
                
            }

            context.EnterClosureContext();

            context.PushTranslated(fde);
        }

        public override void OnAfterChildTraversal(TranslationContext context, SimpleLambdaExpressionSyntax originalNode)
        {
            context.PopTranslated();
            context.ExitClosureContext();
        }
    }
}
