using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.If
{
    public class ElseClauseSyntaxReactor : ReactorBase<ElseClauseSyntax>
    {
        protected override void HandleNode(ElseClauseSyntax node, TranslationContext context, TranslationResult result)
        {
            if (node.Statement is IfStatementSyntax) // handle elseifs
            {
                result.Strategy = TranslationStrategy.DontTraverseChildren;
                result.PrepareForManualPush(context);
                var ifst = (IfStatementSyntax) node.Statement;
                ifst = ifst.WithCondition(SyntaxFactory.ParenthesizedExpression(ifst.Condition));
                if (ifst.Else != null) context.TranslationStack.Push(ifst.Else);
                context.TranslationStack.Push(ifst.Statement);
                context.TranslationStack.Push(ifst.Condition);
            }
            else
            {
                result.Strategy = TranslationStrategy.DontTraverseChildren;
                result.PrepareForManualPush(context);
                context.TranslationStack.Push(node.Statement);
            }
        }
    }
}