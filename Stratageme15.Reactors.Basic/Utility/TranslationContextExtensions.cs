using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Utility
{
    public static class TranslationContextExtensions
    {
        public static string JavascriptCurrentTypeName(this TranslationContextWrapper ctx)
        {
            return ctx.CurrentClassContext.OriginalNode.Name();
        }

        public static bool IsMethodOfThis(this TranslationContextWrapper ctx, string methodName)
        {
            if (ctx.CurrentClassContext == null) return false;
            return ctx.CurrentClassContext.Type.IsMethodOfThis(methodName);
        }

        public static bool IsThisFieldOrPropertyVariable(this TranslationContextWrapper ctx, string varName)
        {
            if (ctx.CurrentClassContext == null) return false;
            return ctx.CurrentClassContext.Type.IsThisFieldOrPropertyVariable(varName);
        }

        public static bool IsThisFieldVariable(this TranslationContextWrapper ctx, string varName)
        {
            if (ctx.CurrentClassContext == null) return false;
            return ctx.CurrentClassContext.Type.IsThisFieldVariable(varName);
        }

        public static bool IsProperty(this TranslationContextWrapper ctx, string varName)
        {
            if (ctx.CurrentClassContext == null) return false;
            return ctx.CurrentClassContext.Type.IsProperty(varName);
        }

        public static bool IsAutoProperty(this TranslationContextWrapper ctx, string varName)
        {
            if (ctx.CurrentClassContext == null) return false;
            return ctx.CurrentClassContext.Type.IsAutoProperty(varName);
        }

        private const string ClosureContextVariable = "CLOSURE_CONTEXT";
        public static void EnterClosureContext(this TranslationContextWrapper ctx)
        {
            ctx.CurrentClassContext[ClosureContextVariable] = true;
        }

        public static void ExitClosureContext(this TranslationContextWrapper ctx)
        {
            ctx.CurrentClassContext.DropCustomVariable(ClosureContextVariable);
        }

        public static bool IsInClosureContext(this TranslationContextWrapper ctx)
        {
            return ctx.CurrentClassContext.IsCustomVariableDefined(ClosureContextVariable);
        }

        public static Type InferLambdaArgumentType(this TranslationContext ctx,SimpleLambdaExpressionSyntax node,string argName)
        {
            var np = node.Parent;
            //todo infer argument type
            
            return null;

        }
    }
}