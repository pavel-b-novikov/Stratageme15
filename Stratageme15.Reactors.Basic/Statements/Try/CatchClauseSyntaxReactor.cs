using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Builders;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.Try
{
    public class CatchClauseSyntaxReactor : ReactorBase<CatchClauseSyntax>
    {
        protected override void HandleNode(CatchClauseSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            result.PrepareForManualPush(context);
            
            CatchClause ctch = new CatchClause();
            context.TranslatedNode.CollectSymbol(ctch);
            if(node.Declaration!=null)
            {
                if (node.Declaration.Identifier.Kind != SyntaxKind.None)
                {
                    ctch.Identifier = node.Declaration.Identifier.ValueText.Ident();
                    context.CurrentClassContext.CurrentFunction.LocalVariables.PushContext();
                    var exctype = TypeInferer.GetTypeFromContext(node.Declaration.Type,context);
                    context.CurrentClassContext.CurrentFunction.LocalVariables.DefineVariable(node.Declaration.Identifier.ValueText,exctype);
                }

            }
            context.PushTranslated(ctch);
            context.TranslationStack.Push(node.Block);
        }

        public override void OnAfterChildTraversal(TranslationContext context, CatchClauseSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.PopTranslated();
            if (originalNode.Declaration!=null)
            {
                context.CurrentClassContext.CurrentFunction.LocalVariables.PopContext();
            }
        }
    }
}
