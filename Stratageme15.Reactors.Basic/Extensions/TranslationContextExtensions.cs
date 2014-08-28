using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Extensions
{
    public static class TranslationContextExtensions
    {
        public static string JavascriptCurrentTypeName(this TranslationContext ctx)
        {
            return TypeExtensions.JavascriptTypeName(ctx.Namespace, ctx.CurrentTypeName);
        }


        public static bool IsMethodOfThis(this TranslationContext ctx, string methodName)
        {
            if (ctx.CurrentClassContext == null) return false;
            return ctx.CurrentClassContext.Type.IsMethodOfThis(methodName);
        }

        public static bool IsThisFieldOrPropertyVariable(this TranslationContext ctx, string varName)
        {
            if (ctx.CurrentClassContext == null) return false;
            return ctx.CurrentClassContext.Type.IsThisFieldOrPropertyVariable(varName);
        }

        public static bool IsThisFieldVariable(this TranslationContext ctx, string varName)
        {
            if (ctx.CurrentClassContext == null) return false;
            return ctx.CurrentClassContext.Type.IsThisFieldVariable(varName);
        }

        public static bool IsProperty(this TranslationContext ctx, string varName)
        {
            if (ctx.CurrentClassContext == null) return false;
            return ctx.CurrentClassContext.Type.IsProperty(varName);
        }

        public static bool IsAutoProperty(this TranslationContext ctx, string varName)
        {
            if (ctx.CurrentClassContext == null) return false;
            return ctx.CurrentClassContext.Type.IsAutoProperty(varName);
        }

        private const string ClosureContextVariable = "CLOSURE_CONTEXT";
        public static void EnterClosureContext(this TranslationContext ctx)
        {
            ctx.CurrentClassContext[ClosureContextVariable] = true;
        }

        public static void ExitClosureContext(this TranslationContext ctx)
        {
            ctx.CurrentClassContext.DropCustomVariable(ClosureContextVariable);
        }

        public static bool IsInClosureContext(this TranslationContext ctx)
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