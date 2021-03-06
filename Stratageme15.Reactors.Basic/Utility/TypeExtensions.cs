﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Stratageme15.Reactors.Basic.Utility
{
    public static class TypeExtensions
    {
        public static bool IsMethodOfThis(this ITypeSymbol t, string methodName)
        {
            var members = t.GetMembers(methodName).Where(c=>c.Kind==SymbolKind.Method);
            return members.Any();
        }

        public static bool IsThisFieldVariable(this ITypeSymbol t, string varName)
        {
            return t.GetMembers(varName).Any(c => c.Kind == SymbolKind.Field);
        }

        public static bool IsThisFieldOrPropertyVariable(this ITypeSymbol t, string varName)
        {
            return t.GetMembers(varName).Any(c => c.Kind == SymbolKind.Field||c.Kind==SymbolKind.Property);
        }

        public static bool IsProperty(this ITypeSymbol t, string varName)
        {
            return t.GetMembers(varName).Any(c => c.Kind == SymbolKind.Property);
        }

        public static bool IsAutoProperty(this ITypeSymbol t, string propName)
        {
            return false;//todo
            //var symbols = t.ConvertedType.GetMembers(propName).Where(c=>c.Kind==)
            //MemberInfo member = TypeInferer.GetMember(t, propName);
            //if (member == null) throw new Exception(string.Format("No member with name {0} in type {1}", propName, t));
            //if (member.MemberType != MemberTypes.Property)
            //    throw new Exception(string.Format("Member {0} if type {1} is not property", propName, t));
            //var pinfo = member as PropertyInfo;
            //return pinfo.IsPropertyAutoGenerated();
        }

        public static bool IsPropertyAutoGenerated(this PropertyInfo pi)
        {
            bool autogenGetter = pi.GetGetMethod().GetCustomAttribute<CompilerGeneratedAttribute>() != null;
            if (!autogenGetter) return false;

            if (pi.DeclaringType == null) return true;

            bool autogenBacking = pi.DeclaringType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.Name.Contains(pi.Name))
                .Where(f => f.Name.Contains("BackingField")).Any(
                    f => f.GetCustomAttributes(typeof (CompilerGeneratedAttribute), true).Any());

            return autogenBacking;
        }

        public static string FullQualifiedName(this ClassDeclarationSyntax node)
        {
            Stack<string> ns = new Stack<string>();
            ns.Push(node.Identifier.ValueText);
            SyntaxNode parent = node.Parent;
            while (parent!=null)
            {
                if (parent is ClassDeclarationSyntax)
                {
                    ns.Push(((ClassDeclarationSyntax)parent).Identifier.ValueText);
                }
                if (parent is NamespaceDeclarationSyntax)
                {
                    ns.Push(((NamespaceDeclarationSyntax)parent).Name.ToString());
                }
                parent = parent.Parent;
            }
            StringBuilder sb = new StringBuilder();
            while(ns.Count>1)
            {
                sb.AppendFormat("{0}.",ns.Pop());
            }
            sb.Append(ns.Pop());
            return sb.ToString();
        }

        public static string FullQualifiedName(this NamespaceDeclarationSyntax node)
        {
            Stack<string> ns = new Stack<string>();
            ns.Push(node.Name.ToString());
            SyntaxNode parent = node.Parent;
            while (parent != null)
            {
                if (parent is NamespaceDeclarationSyntax)
                {
                    ns.Push(((NamespaceDeclarationSyntax)parent).Name.ToString());
                }
                parent = parent.Parent;
            }
            StringBuilder sb = new StringBuilder();
            while (ns.Count > 1)
            {
                sb.AppendFormat("{0}.", ns.Pop());
            }
            sb.Append(ns.Pop());
            return sb.ToString();
        }

        public static string FullNamespace(this ClassDeclarationSyntax node)
        {
            Stack<string> ns = new Stack<string>();
            SyntaxNode parent = node.Parent;
            while (parent != null)
            {
                if (parent is NamespaceDeclarationSyntax)
                {
                    ns.Push(((NamespaceDeclarationSyntax)parent).Name.ToString());
                }
                parent = parent.Parent;
            }
            if (ns.Count == 0) return string.Empty;
            StringBuilder sb = new StringBuilder();
            while (ns.Count > 1)
            {
                sb.AppendFormat("{0}.", ns.Pop());
            }
            sb.Append(ns.Pop());
            return sb.ToString();
        }

        public static string FullQualifiedName(this ITypeSymbol node)
        {
            Stack<string> ns = new Stack<string>();
            ns.Push(node.Name);
            INamespaceSymbol parent = node.ContainingNamespace;
            while (parent != null||!parent.IsGlobalNamespace)
            {
                ns.Push(parent.Name);
                parent = parent.ContainingNamespace;
            }
            StringBuilder sb = new StringBuilder();
            while (ns.Count > 1)
            {
                sb.AppendFormat("{0}.", ns.Pop());
            }
            sb.Append(ns.Pop());
            return sb.ToString();
        }

        public static string FullNamespace(this ITypeSymbol node)
        {
            Stack<string> ns = new Stack<string>();
            INamespaceSymbol parent = node.ContainingNamespace;
            while (parent != null && !parent.IsGlobalNamespace)
            {
                ns.Push(parent.Name);
                parent = parent.ContainingNamespace;
            }
            if (ns.Count == 0) return string.Empty;
            StringBuilder sb = new StringBuilder();
            while (ns.Count > 1)
            {
                sb.AppendFormat("{0}.", ns.Pop());
            }
            sb.Append(ns.Pop());
            return sb.ToString();
        }
    }
}