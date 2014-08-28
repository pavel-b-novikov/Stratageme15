using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.ExtensionsPack.Tactics.Base
{
    public  abstract partial class ClassTranslationTacticsBase
    {
        public abstract void DeclarativeClass(ClassDeclarationSyntax node, TranslationContext ctx);

        public abstract void ImperativeClass(IdentifierNameSyntax node, TranslationContext ctx);
    }
}
