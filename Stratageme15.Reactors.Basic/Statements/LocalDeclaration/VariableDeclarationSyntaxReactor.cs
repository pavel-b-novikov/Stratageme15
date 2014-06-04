using System;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.LocalDeclaration
{
    public class VariableDeclarationSyntaxReactor : ReactorBase<VariableDeclarationSyntax>
    {
        protected override void HandleNode(VariableDeclarationSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;

            //node.Declaration.Type haha! we dont need it in javascript!!
            // jk. additional types will be handled later in other reactor batches
            // (after little bit digging)
            // well. it seemes that we still need types for local variables context
            VariableDefStatement vds = new VariableDefStatement();
            context.PushTranslated(vds);

            var lvc = context.CurrentClassContext.CurrentFunction.LocalVariables;
            
            if (node.Type.IsVar)
            {
                lvc.PromiseType();
            }else
            {
                var t = TypeInferer.GetTypeFromContext(node.Type, context);

                lvc.StartDeclaringWithType(t);
            }
        }

        public override void OnAfterChildTraversal(TranslationContext context, VariableDeclarationSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);

            var lvc = context.CurrentClassContext.CurrentFunction.LocalVariables;
            if (lvc.IsNextDeclarationsTypeSet)
            {
                lvc.StopDeclaringWithType();
            }

            var vds = context.TranslatedNode;
            context.PopTranslated();
            context.TranslatedNode.CollectSymbol(vds);


        }
    }
}
