using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Contexts;
using Microsoft.CodeAnalysis;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements.LocalDeclaration
{
    public class VariableDeclaratorSyntaxReactor : BasicReactorBase<VariableDeclaratorSyntax>
    {
        protected override void HandleNode(VariableDeclaratorSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            VariablesContext lvc = context.CurrentClassContext.CurrentFunction.LocalVariables;
            lvc.DefineVariable(node.Identifier.ValueText);

            if (node.Initializer == null)
            {
                result.Strategy = TranslationStrategy.DontTraverseChildren;
                context.Context.TargetNode.CollectSymbol(node.Identifier.ValueText.ToIdent());
            }
            else
            {
                result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
                var a = new AssignmentStatement();
                a.CollectSymbol(node.Identifier.ValueText.ToIdent());
                context.Context.PushTranslated(a);
                if (lvc.IsTypePromised)
                {
                    lvc.ResolveTypeForPromisees(context.Context.SemanticModel.GetTypeInfo(node.Initializer.Value));
                }
            }
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, VariableDeclaratorSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            SyntaxTreeNodeBase assignmentStatement = context.Context.TargetNode;
            context.Context.PopTranslated();
            context.Context.TargetNode.CollectSymbol(assignmentStatement);
        }
    }
}