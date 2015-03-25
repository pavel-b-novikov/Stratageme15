using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Declarations
{
    internal class StatementBlockSyntaxReactor : BasicReactorBase<BlockSyntax>, ISituationReactor
    {
        #region ISituationReactor Members

        public bool IsAcceptable(TranslationContext context)
        {
            return (
                       (context.TargetNode is IStatement)
                       || (context.TargetNode is CatchClause)
                       || (context.TargetNode is FinallyClause)
                   )
                   && (!(context.TargetNode is CodeBlock))
                   && (!(context.TargetNode is FunctionDefExpression))
                ;
        }

        #endregion

        protected override void HandleNode(BlockSyntax node, TranslationContextWrapper context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            var code = new CodeBlock();
            context.Context.TargetNode.CollectSymbol(code);
            context.Context.PushTranslated(code);

            context.CurrentClassContext.CurrentFunction.LocalVariables.PushContext();
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, BlockSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.CurrentClassContext.CurrentFunction.LocalVariables.PopContext();
            context.Context.PopTranslated();
        }
    }
}