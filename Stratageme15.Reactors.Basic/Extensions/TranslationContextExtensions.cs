using System;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Extensions
{
    public static class TranslationContextExtensions
    {
        public static string JavascriptCurrentTypeName(this TranslationContext ctx)
        {
            return JavascriptTypeName(ctx.Namespace, ctx.CurrentTypeName);
        }

        public static string JavascriptTypeName(string nameSpace,string typeName)
        {
            return string.Format("{0}${1}", nameSpace.Replace(".", string.Empty), typeName);
        }

        public static string JavascriptTypeName(this Type t)
        {
            return JavascriptTypeName(t.Namespace, t.Name);
        }
    }
}
