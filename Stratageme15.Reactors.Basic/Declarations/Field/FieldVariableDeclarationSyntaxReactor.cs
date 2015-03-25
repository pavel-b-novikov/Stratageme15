using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Declarations.Field
{
    internal class FieldVariableDeclarationSyntaxReactor : BasicReactorBase<VariableDeclarationSyntax>, ISituationReactor
    {
        #region ISituationReactor Members

        public bool IsAcceptable(TranslationContext context)
        {
            return context.SourceNode.Parent is FieldDeclarationSyntax;
        }

        #endregion

        protected override void HandleNode(VariableDeclarationSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            TypeInfo type = context.Context.SemanticModel.GetTypeInfo(node.Type); //no cases when here will be var //todo
            result.Strategy = TranslationStrategy.TraverseChildren;
            result.PrepareForManualPush(context.Context);
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
                context.Context.TranslationStack.Push(toPush);
            }
        }
    }
}