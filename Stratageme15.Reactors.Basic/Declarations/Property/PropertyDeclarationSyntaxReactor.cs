using System.Linq;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Declarations.Property
{
    public class PropertyDeclarationSyntaxReactor : ReactorBase<PropertyDeclarationSyntax>
    {
        protected override void HandleNode(PropertyDeclarationSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            result.PrepareForManualPush(context);
            bool isAuto = node.AccessorList.Accessors.All(c => c.Body == null);
            var backingAccessor =Syntax.MemberAccessExpression(
                SyntaxKind.MemberAccessExpression,
                Syntax.ThisExpression(),
                Syntax.IdentifierName(
                    string.Format("_{0}", node.Identifier.ValueText)));
            foreach (var accessorDeclarationSyntax in node.AccessorList.Accessors)
            {
                string accPrefix = accessorDeclarationSyntax.Kind == SyntaxKind.GetAccessorDeclaration ? "get" : "set";
                string accName = string.Format("{0}{1}", accPrefix, node.Identifier.ValueText);
                MethodDeclarationSyntax method = null;
                if (accessorDeclarationSyntax.Kind == SyntaxKind.SetAccessorDeclaration)
                {
                    method = Syntax.MethodDeclaration(Syntax.PredefinedType(
                            Syntax.Token(
                                SyntaxKind.VoidKeyword)), accName)
                        .WithModifiers(node.Modifiers)
                        .WithParameterList(Syntax.ParameterList(
                        Syntax.SeparatedList<ParameterSyntax>(Syntax.Parameter(Syntax.Identifier("value"))
                                                                  .WithType(node.Type))));
                }
                else
                {
                    method = Syntax.MethodDeclaration(node.Type, accName).
                        WithModifiers(
                            node.Modifiers);
                }
               
                if (!isAuto)
                {
                    method = method.WithBody(accessorDeclarationSyntax.Body);
                }else
                {
                    if (accessorDeclarationSyntax.Kind == SyntaxKind.GetAccessorDeclaration)
                    {
                        method = method.WithBody(
                            Syntax.Block(
                                Syntax.List<StatementSyntax>(
                                    Syntax.ReturnStatement(backingAccessor))));
                    }else
                    {
                        method = method.WithBody(
                            Syntax.Block(
                                Syntax.List<StatementSyntax>(
                                    Syntax.ExpressionStatement(
                                        Syntax.BinaryExpression(
                                            SyntaxKind.AssignExpression,
                                            backingAccessor,
                                            Syntax.IdentifierName(
                                                @"value"))))));
                    }
                }

                context.TranslationStack.Push(method);
            }

            if (isAuto)
            {
                //backing field
                var bf = Syntax.FieldDeclaration(
                    Syntax.VariableDeclaration(node.Type)
                        .WithVariables(Syntax.SeparatedList(Syntax.VariableDeclarator(Syntax.Identifier(string.Format("_{0}", node.Identifier.ValueText))))))
                        .WithModifiers(Syntax.TokenList(Syntax.Token(SyntaxKind.PrivateKeyword)));

                context.TranslationStack.Push(bf);
            }


        }
    }
}
