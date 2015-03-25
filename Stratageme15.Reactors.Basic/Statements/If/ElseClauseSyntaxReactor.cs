using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements.If
{
    public class ElseClauseSyntaxReactor : BasicReactorBase<ElseClauseSyntax>
    {
        protected override void HandleNode(ElseClauseSyntax node, TranslationContextWrapper context, TranslationResult result)
        {
            if (node.Statement is IfStatementSyntax) // handle elseifs
            {
                result.Strategy = TranslationStrategy.DontTraverseChildren;
                result.PrepareForManualPush(context.Context);
                var ifst = (IfStatementSyntax) node.Statement;
                ifst = ifst.WithCondition(SyntaxFactory.ParenthesizedExpression(ifst.Condition));
                if (ifst.Else != null) context.Context.TranslationStack.Push(ifst.Else);
                context.Context.TranslationStack.Push(ifst.Statement);
                context.Context.TranslationStack.Push(ifst.Condition);
            }
            else
            {
                result.Strategy = TranslationStrategy.DontTraverseChildren;
                result.PrepareForManualPush(context.Context);
                context.Context.TranslationStack.Push(node.Statement);
            }
        }
    }
}