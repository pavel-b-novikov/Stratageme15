using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.ExtensionsPack.Tactics.Base
{
    public abstract partial class ClassTranslationTacticsBase
    {
        public abstract void DeclarativeStaticConstructor(ConstructorDeclarationSyntax node, TranslationContext ctx);

        public abstract void DeclarativeStaticField(FieldDeclarationSyntax node, TranslationContext ctx);

        public abstract void DeclarativeStaticProperty(PropertyDeclarationSyntax node, TranslationContext ctx);

        public abstract void DeclarativeStaticMethod(MethodDeclarationSyntax node, TranslationContext ctx);

        public abstract void DeclarativeStaticEvent(EventDeclarationSyntax node, TranslationContext ctx);
    }
}
