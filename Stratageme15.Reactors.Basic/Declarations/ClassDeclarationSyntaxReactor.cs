using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Binary;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Builders;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class ClassDeclarationSyntaxReactor : ReactorBase<ClassDeclarationSyntax>
    {
        protected override void HandleNode(ClassDeclarationSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;

            var className = node.Identifier.Value;
            var fullTypeName = string.Format("{0}.{1}", context.Namespace, className);
            var type = context.Assemblies.GetType(fullTypeName);

            context.PushClass(new ClassTranslationContext(node,type));
        }

        public override void OnAfterChildTraversal(TranslationContext context,ClassDeclarationSyntax originalNode)
        {
            AssignmentBinaryExpression abe = 
                context.JavascriptCurrentTypeName()
                    .MemberAccess()
                        .Field("prototype")
                        .Field("constructor")
                        .Build()
                    .Assignment(context.JavascriptCurrentTypeName());
            
            context.TranslatedNode.CollectSymbol(abe);
            context.TranslatedNode.EmptyColon();
            context.PopClass();
        }
    }
}
