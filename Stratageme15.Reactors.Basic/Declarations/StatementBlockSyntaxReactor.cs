using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Declarations
{
    internal class StatementBlockSyntaxReactor : ReactorBase<BlockSyntax>, ISituationReactor
    {
        #region ISituationReactor Members

        public bool IsAcceptable(TranslationContext context)
        {
            return (
                       (context.TranslatedNode is IStatement)
                       || (context.TranslatedNode is CatchClause)
                       || (context.TranslatedNode is FinallyClause)
                   )
                   && (!(context.TranslatedNode is CodeBlock))
                   && (!(context.TranslatedNode is FunctionDefExpression))
                ;
        }

        #endregion

        protected override void HandleNode(BlockSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            var code = new CodeBlock();
            context.TranslatedNode.CollectSymbol(code);
            context.PushTranslated(code);

            context.CurrentClassContext.CurrentFunction.LocalVariables.PushContext();
        }

        public override void OnAfterChildTraversal(TranslationContext context, BlockSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.CurrentClassContext.CurrentFunction.LocalVariables.PopContext();
            context.PopTranslated();
        }
    }
}