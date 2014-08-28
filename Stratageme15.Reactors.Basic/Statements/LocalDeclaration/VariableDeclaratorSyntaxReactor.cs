using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Builders;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.LocalDeclaration
{
    public class VariableDeclaratorSyntaxReactor : ReactorBase<VariableDeclaratorSyntax>
    {
        protected override void HandleNode(VariableDeclaratorSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            VariablesContext lvc = context.CurrentClassContext.CurrentFunction.LocalVariables;
            lvc.DefineVariable(node.Identifier.ValueText);

            if (node.Initializer == null)
            {
                result.Strategy = TranslationStrategy.DontTraverseChildren;
                context.TranslatedNode.CollectSymbol(node.Identifier.ValueText.Ident());
            }
            else
            {
                result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
                var a = new AssignmentStatement();
                a.CollectSymbol(node.Identifier.ValueText.Ident());
                context.PushTranslated(a);
                if (lvc.IsTypePromised)
                {
                    lvc.ResolveTypeForPromisees(TypeInferer.InferTypeFromExpression(node.Initializer.Value, context));
                }
            }
        }

        public override void OnAfterChildTraversal(TranslationContext context, VariableDeclaratorSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            SyntaxTreeNodeBase assignmentStatement = context.TranslatedNode;
            context.PopTranslated();
            context.TranslatedNode.CollectSymbol(assignmentStatement);
        }
    }
}