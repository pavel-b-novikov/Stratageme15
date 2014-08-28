using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Builders;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class AnonymousObjectMemberDeclaratorSyntaxReactor :
        ExpressionReactorBase<AnonymousObjectMemberDeclaratorSyntax, ObjectFieldDef>
    {
        public override ObjectFieldDef TranslateNodeInner(AnonymousObjectMemberDeclaratorSyntax node,
                                                          TranslationContext context, TranslationResult result)
        {
            result.PrepareForManualPush(context);
            result.Strategy = TranslationStrategy.TraverseChildren;
            var o = new ObjectFieldDef();
            o.Key = node.NameEquals.Name.Identifier.ValueText.Ident();
            context.TranslationStack.Push(node.Expression);
            return o;
        }
    }
}