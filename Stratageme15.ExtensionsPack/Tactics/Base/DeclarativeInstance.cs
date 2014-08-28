using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.ExtensionsPack.Tactics.Base
{
    public abstract partial class ClassTranslationTacticsBase
    {
        public abstract void DeclarativeInstanceConstructor(ConstructorDeclarationSyntax node, TranslationContext ctx);

        public abstract void DeclarativeInstanceField(FieldDeclarationSyntax node, TranslationContext ctx);

        public abstract void DeclarativeInstanceProperty(PropertyDeclarationSyntax node, TranslationContext ctx);

        public abstract void DeclarativeInstanceMethod(MethodDeclarationSyntax node, TranslationContext ctx);

        public abstract void DeclarativeInstanceEvent(EventDeclarationSyntax node, TranslationContext ctx);
    }
}
