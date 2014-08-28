using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.ExtensionsPack.Tactics.Base
{
    public abstract partial class ClassTranslationTacticsBase
    {
        public abstract bool ImperativeStaticMethodCall(InvocationExpressionSyntax node, TranslationContext ctx);
        
        public abstract bool ImperativeStaticFieldAccess(MemberAccessExpressionSyntax node, TranslationContext ctx);
        
        public abstract bool ImperativeStaticPropertyAccess(MemberAccessExpressionSyntax node, TranslationContext ctx);

        public abstract bool ImperativeStaticConstAccess(MemberAccessExpressionSyntax node, TranslationContext ctx);

        //BinaryExpressionSyntax

        public abstract bool ImperativeStaticEventAdd(BinaryExpressionSyntax node, TranslationContext ctx);

        public abstract bool ImperativeStaticEventRemove(BinaryExpressionSyntax node, TranslationContext ctx);

        public abstract bool ImperativeStaticEventInvoke(BinaryExpressionSyntax node, TranslationContext ctx);

    }
}
