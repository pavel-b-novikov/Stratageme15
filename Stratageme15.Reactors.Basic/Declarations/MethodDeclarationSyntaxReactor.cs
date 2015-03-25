using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Binary;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Translation;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class MethodDeclarationSyntaxReactor : BasicReactorBase<MethodDeclarationSyntax>
    {
        protected override void HandleNode(MethodDeclarationSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            context.CurrentClassContext.PushFunction(node, node.Identifier.ValueText);
            context.Context.PushTranslated(context.CurrentClassContext.CurrentFunction.Function);
            result.PrepareForManualPush(context.Context);
            context.Context.TranslationStack.Push(node.Body);
            context.Context.TranslationStack.Push(node.ParameterList);
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, MethodDeclarationSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            FunctionDefExpression fn = context.CurrentClassContext.CurrentFunction.Function;
            string name = fn.Name.Identifier;
            fn.Name = null;

            context.CurrentClassContext.PopFunction();
            context.Context.PopTranslated();

            
            Expression a = context.JavascriptCurrentTypeName().ToIdent();
            if (!originalNode.Modifiers.Any(SyntaxKind.StaticKeyword))
            {
                a = a.Member("prototype");
            }
            AssignmentBinaryExpression abe = a.Member(name).Assignment(fn);

            context.Context.TargetNode.CollectSymbol(abe);
        }
    }
}