using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Declarations.Field
{
    internal class FieldVariableDeclarationSyntaxReactor : ReactorBase<VariableDeclarationSyntax>, ISituationReactor
    {
        #region ISituationReactor Members

        public bool IsAcceptable(TranslationContext context)
        {
            return context.SourceNode.Parent is FieldDeclarationSyntax;
        }

        #endregion

        protected override void HandleNode(VariableDeclarationSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            Type type = TypeInferer.GetTypeFromContext(node.Type, context); //no cases when here will be var
            result.Strategy = TranslationStrategy.TraverseChildren;
            result.PrepareForManualPush(context);
            LiteralExpressionSyntax def = type.GetDefaultValueSyntax();

            foreach (VariableDeclaratorSyntax variableDeclaratorSyntax in node.Variables)
            {
                SyntaxNode toPush = null;
                if (variableDeclaratorSyntax.Initializer != null)
                {
                    toPush = SyntaxFactory.BinaryExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName(variableDeclaratorSyntax.Identifier)),
                        variableDeclaratorSyntax.Initializer.Value);
                }
                else
                {
                    toPush = SyntaxFactory.BinaryExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName(variableDeclaratorSyntax.Identifier)),
                        def);
                }
                context.TranslationStack.Push(toPush);
            }
        }
    }
}