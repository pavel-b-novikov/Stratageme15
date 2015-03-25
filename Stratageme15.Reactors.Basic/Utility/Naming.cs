using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Stratageme15.Reactors.Basic.Utility
{
    public static class Naming
    {
        public static string Name(this ClassDeclarationSyntax ti)
        {
            return Name(ti.FullNamespace(), ti.Identifier.ValueText);
        }

        public static string Name(this ITypeSymbol ti)
        {
            return Name(ti.FullNamespace(), ti.Name);
        }

        public static string Name(string nameSpace, string typeName)
        {
            if (string.IsNullOrEmpty(nameSpace)) return typeName;
            return String.Format("{0}${1}", nameSpace.Replace(".", "_"), typeName);
        }
    }
}
