using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Binary;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Builders;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class MethodDeclarationSyntaxReactor : ReactorBase<MethodDeclarationSyntax>
    {
        protected override void HandleNode(MethodDeclarationSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            context.CurrentClassContext.PushFunction(node,node.Identifier.ValueText);
            context.PushTranslated(context.CurrentClassContext.CurrentFunction.Function);
        }

        public override void OnAfterChildTraversal(TranslationContext context, MethodDeclarationSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            var fn = context.CurrentClassContext.CurrentFunction.Function;
            var name = fn.Name.Identifier;
            fn.Name = null;

            context.CurrentClassContext.PopFunction();
            context.PopTranslated();

            AssignmentBinaryExpression abe =
              context.JavascriptCurrentTypeName()
                  .MemberAccess()
                      .Field("prototype")
                      .Field(name)
                      .Build()
                  .Assignment(fn);

            context.TranslatedNode.CollectSymbol(abe);
        }
    }
}
