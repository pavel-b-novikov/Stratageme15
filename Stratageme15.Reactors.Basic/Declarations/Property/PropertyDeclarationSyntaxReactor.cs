using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Declarations.Property
{
    public class PropertyDeclarationSyntaxReactor : ReactorBase<PropertyDeclarationSyntax>
    {
        protected override void HandleNode(PropertyDeclarationSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            if (node.Modifiers.Any(SyntaxKind.StaticKeyword))
                throw new Exception("Static properties are not supported");
            result.Strategy = TranslationStrategy.TraverseChildren;
            result.PrepareForManualPush(context);
            bool isAuto = node.AccessorList.Accessors.All(c => c.Body == null);
            MemberAccessExpressionSyntax backingAccessor = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.ThisExpression(),
                SyntaxFactory.IdentifierName(
                    string.Format("_{0}", node.Identifier.ValueText)));
            foreach (AccessorDeclarationSyntax accessorDeclarationSyntax in node.AccessorList.Accessors)
            {
                string accPrefix = accessorDeclarationSyntax.CSharpKind() == SyntaxKind.GetAccessorDeclaration
                                       ? "get"
                                       : "set";
                string accName = string.Format("{0}{1}", accPrefix, node.Identifier.ValueText);
                MethodDeclarationSyntax method = null;
                if (accessorDeclarationSyntax.CSharpKind() == SyntaxKind.SetAccessorDeclaration)
                {
                    method = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(
                        SyntaxFactory.Token(
                            SyntaxKind.VoidKeyword)), accName)
                        .WithModifiers(node.Modifiers)
                        .WithParameterList(SyntaxFactory.ParameterList(
                            SyntaxFactory.SeparatedList(new[]
                                                            {
                                                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("value"))
                                                                    .WithType(node.Type)
                                                            })));
                }
                else
                {
                    method = SyntaxFactory.MethodDeclaration(node.Type, accName).
                        WithModifiers(
                            node.Modifiers);
                }

                if (!isAuto)
                {
                    method = method.WithBody(accessorDeclarationSyntax.Body);
                }
                else
                {
                    if (accessorDeclarationSyntax.CSharpKind() == SyntaxKind.GetAccessorDeclaration)
                    {
                        method = method.WithBody(
                            SyntaxFactory.Block(
                                SyntaxFactory.List<StatementSyntax>(
                                    new[] {SyntaxFactory.ReturnStatement(backingAccessor)})));
                    }
                    else
                    {
                        method = method.WithBody(
                            SyntaxFactory.Block(
                                SyntaxFactory.List<StatementSyntax>(new[]
                                                                        {
                                                                            SyntaxFactory.ExpressionStatement(
                                                                                SyntaxFactory.BinaryExpression(
                                                                                    SyntaxKind.
                                                                                        SimpleAssignmentExpression,
                                                                                    backingAccessor,
                                                                                    SyntaxFactory.IdentifierName(
                                                                                        @"value")))
                                                                        })));
                    }
                }

                context.TranslationStack.Push(method);
            }

            if (isAuto)
            {
                //backing field
                FieldDeclarationSyntax bf = SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(node.Type)
                        .WithVariables(
                            SyntaxFactory.SeparatedList(new[]
                                                            {
                                                                SyntaxFactory.VariableDeclarator(
                                                                    SyntaxFactory.Identifier(string.Format("_{0}",
                                                                                                           node.
                                                                                                               Identifier
                                                                                                               .
                                                                                                               ValueText)))
                                                            })))
                    .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)));

                context.TranslationStack.Push(bf);
            }
        }
    }
}