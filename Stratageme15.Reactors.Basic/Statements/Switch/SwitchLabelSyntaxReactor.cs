using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements.Switch
{
    public class SwitchLabelSyntaxReactor : BasicReactorBase<SwitchLabelSyntax>
    {
        protected override void HandleNode(SwitchLabelSyntax node, TranslationContextWrapper context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            SyntaxTreeNodeBase snb = null;
            if (node.CaseOrDefaultKeyword.CSharpKind() == SyntaxKind.CaseKeyword)
            {
                snb = new CaseClause();
            }
            else
            {
                snb = new DefaultClause();
            }
            context.Context.TargetNode.CollectSymbol(snb);
            context.Context.PushTranslated(snb);

            var parent = (SwitchSectionSyntax) node.Parent;
            if (node == parent.Labels.Last())
            {
                result.PrepareForManualPush(context.Context);
                foreach (StatementSyntax statement in parent.Statements.Reverse())
                {
                    context.Context.TranslationStack.Push(statement);
                }
                if (node.Value != null) context.Context.TranslationStack.Push(node.Value);
            }
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, SwitchLabelSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.Context.PopTranslated();
        }
    }
}