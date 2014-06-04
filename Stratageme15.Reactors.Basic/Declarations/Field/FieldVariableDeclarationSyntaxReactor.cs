using Roslyn.Compilers.CSharp;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Declarations.Field
{
    class FieldVariableDeclarationSyntaxReactor : ReactorBase<VariableDeclarationSyntax>, ISituationReactor
    {
        protected override void HandleNode(VariableDeclarationSyntax node, TranslationContext context, TranslationResult result)
        {
            var type = TypeInferer.GetTypeFromContext(node.Type, context); //no cases when here will be var
            result.Strategy = TranslationStrategy.TraverseChildren;
            result.PrepareForManualPush(context);
            var def = type.GetDefaultValueSyntax();
            foreach (var variableDeclaratorSyntax in node.Variables) //todo order fields in case of usage one field inside another
            {
                SyntaxNode toPush = null;
                if (variableDeclaratorSyntax.Initializer!=null)
                {
                    toPush = Syntax.BinaryExpression(
                        SyntaxKind.AssignExpression,
                        Syntax.MemberAccessExpression(
                            SyntaxKind.MemberAccessExpression,
                            Syntax.ThisExpression(),
                            Syntax.IdentifierName(variableDeclaratorSyntax.Identifier)),
                        variableDeclaratorSyntax.Initializer.Value);
                }else
                {
                    toPush = Syntax.BinaryExpression(
                        SyntaxKind.AssignExpression,
                        Syntax.MemberAccessExpression(
                            SyntaxKind.MemberAccessExpression,
                            Syntax.ThisExpression(),
                            Syntax.IdentifierName(variableDeclaratorSyntax.Identifier)),
                        def);
                }
                context.TranslationStack.Push(toPush);
            }
        }

        public bool IsAcceptable(TranslationContext context)
        {
            return context.SourceNode.Parent is FieldDeclarationSyntax;
        }
    }
}
