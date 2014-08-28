using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.ExtensionsPack.Tactics.Base
{
    public abstract partial class ClassTranslationTacticsBase
    {
        public abstract bool ImperativeInstanceConstruction(ObjectCreationExpressionSyntax node, TranslationContext ctx);

        public abstract bool ImperativeInstanceVariableDefinition(VariableDeclaratorSyntax node, TranslationContext ctx);

        public abstract bool ImperativeInstanceAsFieldDeclaration(FieldDeclarationSyntax node, TranslationContext ctx);

        public abstract bool ImperativeInstanceAsPropertyDeclaration(PropertyDeclarationSyntax node, TranslationContext ctx);

        //PropertyDeclarationSyntax

        public abstract bool ImperativeInstanceMethodCall(InvocationExpressionSyntax node, TranslationContext ctx);

        public abstract bool ImperativeInstanceFieldAccess(MemberAccessExpressionSyntax node, TranslationContext ctx);

        public abstract bool ImperativeInstancePropertyAccess(MemberAccessExpressionSyntax node, TranslationContext ctx);

        public abstract bool ImperativeInstanceEventAdd(BinaryExpressionSyntax node, TranslationContext ctx);

        public abstract bool ImperativeInstanceEventRemove(BinaryExpressionSyntax node, TranslationContext ctx);
    }
}
