using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class AnonymousObjectMemberDeclaratorSyntaxReactor :
        ExpressionReactorBase<AnonymousObjectMemberDeclaratorSyntax, ObjectFieldDef>
    {
        public override ObjectFieldDef TranslateNodeInner(AnonymousObjectMemberDeclaratorSyntax node,
                                                          TranslationContextWrapper context, TranslationResult result)
        {
            result.PrepareForManualPush(context.Context);
            result.Strategy = TranslationStrategy.TraverseChildren;
            var o = new ObjectFieldDef();
            o.Key = node.NameEquals.Name.Identifier.ValueText.ToIdent();
            context.Context.TranslationStack.Push(node.Expression);
            return o;
        }
    }
}