using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Contexts;
using Stratageme15.Reactors.Basic.Utility;
using TypeInfo = Microsoft.CodeAnalysis.TypeInfo;
namespace Stratageme15.Reactors.Basic.Statements.LocalDeclaration
{
    public class VariableDeclarationSyntaxReactor : BasicReactorBase<VariableDeclarationSyntax>
    {
        protected override void HandleNode(VariableDeclarationSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;

            //node.Declaration.Type haha! we dont need it in javascript!!
            // jk. additional types will be handled later in other reactor batches
            // (after little bit digging)
            // well. it seemes that we still need types for local variables context
            var vds = new VariableDefStatement();
            context.Context.PushTranslated(vds);

            VariablesContext lvc = context.CurrentClassContext.CurrentFunction.LocalVariables;

            if (node.Type.IsVar)
            {
                lvc.PromiseType();
            }
            else
            {
                TypeInfo t = context.Context.SemanticModel.GetTypeInfo(node.Type);

                lvc.StartDeclaringWithType(t);
            }
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, VariableDeclarationSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);

            VariablesContext lvc = context.CurrentClassContext.CurrentFunction.LocalVariables;
            if (lvc.IsNextDeclarationsTypeSet)
            {
                lvc.StopDeclaringWithType();
            }

            SyntaxTreeNodeBase vds = context.Context.TargetNode;
            context.Context.PopTranslated();
            context.Context.TargetNode.CollectSymbol(vds);
        }
    }
}