using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation.Logging;
using TypeInfo = Microsoft.CodeAnalysis.TypeInfo;
// ReSharper disable CanBeReplacedWithTryCastAndCheckForNull

namespace Stratageme15.Reactors.Basic
{
    public static class TypeInfoExtensions
    {
        public static LiteralExpressionSyntax GetDefaultValueSyntax(this TypeInfo t)
        {
            var ct = t.ConvertedType;
            if ((!ct.IsValueType))
                return SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression);
            //todo
            //if (IsPredefinedNumeric(t))
            //{
            //    return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression,
            //                                           SyntaxFactory.Literal(SyntaxFactory.TriviaList(), @"0", 0,
            //                                                                 SyntaxFactory.TriviaList()));
            //}
            //if (t == typeof(bool))
            //{
            //    return SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
            //}
            throw new Exception("unknown type for default expression");
        }
    }
    //public class AnonymousType
    //{
    //}

    ///// <summary>
    ///// Determines .NET type of a Roslyn expression according to current translation context (usings, etc)
    ///// </summary>
    //public static class TypeInferer
    //{
    //    private static readonly Dictionary<string, Type> _predefinedTypes = new Dictionary<string, Type>
    //                                                                            {
    //                                                                                {"string", typeof (string)},
    //                                                                                {"char", typeof (char)},
    //                                                                                {"byte", typeof (byte)},
    //                                                                                {"sbyte", typeof (sbyte)},
    //                                                                                {"ushort", typeof (ushort)},
    //                                                                                {"short", typeof (short)},
    //                                                                                {"uint", typeof (uint)},
    //                                                                                {"int", typeof (int)},
    //                                                                                {"ulong", typeof (ulong)},
    //                                                                                {"long", typeof (long)},
    //                                                                                {"float", typeof (float)},
    //                                                                                {"double", typeof (double)},
    //                                                                                {"decimal", typeof (decimal)},
    //                                                                                {"object", typeof (object)},
    //                                                                                {"bool", typeof (bool)}
    //                                                                            };

    //    public static bool IsPredefinedNumeric(this Type t)
    //    {
    //        return t == typeof (byte)
    //               || t == typeof (sbyte)
    //               || t == typeof (ushort)
    //               || t == typeof (short)
    //               || t == typeof (uint)
    //               || t == typeof (int)
    //               || t == typeof (ulong)
    //               || t == typeof (long)
    //               || t == typeof (float)
    //               || t == typeof (double)
    //               || t == typeof (decimal);
    //    }

   


    //    public static Type GetTypeFromContext(TypeSyntax typeName, TranslationContextWrapper context)
    //    {
    //        if (typeName is GenericNameSyntax) return GetTypeFromContext((GenericNameSyntax) typeName, context);
    //        if (typeName is IdentifierNameSyntax) return GetTypeFromContext((IdentifierNameSyntax) typeName, context);
    //        if (typeName is AliasQualifiedNameSyntax)
    //            return GetTypeFromContext((AliasQualifiedNameSyntax) typeName, context);
    //        if (typeName is QualifiedNameSyntax) return GetTypeFromContext((QualifiedNameSyntax) typeName, context);
    //        if (typeName is PredefinedTypeSyntax) return GetTypeFromContext((PredefinedTypeSyntax) typeName, context);
    //        if (typeName is ArrayTypeSyntax) return GetTypeFromContext((ArrayTypeSyntax) typeName, context);
    //        if (typeName is PointerTypeSyntax) return GetTypeFromContext((PointerTypeSyntax) typeName, context);
    //        if (typeName is NullableTypeSyntax) return GetTypeFromContext((NullableTypeSyntax) typeName, context);

    //        //Well, here we dont actually react OmittedTypeArgumentSyntax 
    //        //because it's usage is strongly restricted and we will handle it
    //        //in special order
    //        context.Crit("Unknown TypeSyntax");
    //        return null;
    //    }

    //    public static Type GetTypeFromContext(GenericNameSyntax typeName, TranslationContextWrapper context)
    //    {
    //        string rawtypeName = typeName.Identifier.ValueText;
    //        int genericsCount = typeName.TypeArgumentList.Arguments.Count;
    //        rawtypeName = string.Format("{0}`{1}", rawtypeName, genericsCount); //to meet "Typename`x" notation

    //        Type rawType = GetTypeFromContext(rawtypeName, context);
    //        var generics = new List<Type>();
    //        foreach (TypeSyntax arg in typeName.TypeArgumentList.Arguments)
    //        {
    //            if (!(arg is OmittedTypeArgumentSyntax))
    //            {
    //                generics.Add(GetTypeFromContext(arg, context));
    //            }
    //        }

    //        if (generics.Count == 0) return rawType;
    //        return rawType.MakeGenericType(generics.ToArray());
    //    }

    //    public static Type GetTypeFromContext(AliasQualifiedNameSyntax typeName, TranslationContextWrapper context)
    //    {
    //        context.Crit("I've just didn't understand when AliasQualifiedNameSyntax actually met");
    //        return null;
    //    }

    //    public static Type GetTypeFromContext(QualifiedNameSyntax typeName, TranslationContextWrapper context)
    //    {
    //        string fullTypeName = typeName.ToFullString();
    //        return GetTypeFromContext(fullTypeName, context);
    //    }

    //    public static Type GetTypeFromContext(PredefinedTypeSyntax typeName, TranslationContextWrapper context)
    //    {
    //        string predefinedName = typeName.Keyword.ValueText;
    //        return _predefinedTypes[predefinedName];
    //    }

    //    public static Type GetTypeFromContext(ArrayTypeSyntax typeName, TranslationContextWrapper context)
    //    {
    //        Type elementType = GetTypeFromContext(typeName.ElementType, context);
    //        Type arrayedType = elementType;
    //        foreach (ArrayRankSpecifierSyntax arrayRankSpecifierSyntax in typeName.RankSpecifiers)
    //        {
    //            arrayedType = arrayedType.MakeArrayType(arrayRankSpecifierSyntax.Rank);
    //        }
    //        return arrayedType;
    //    }

    //    public static Type GetTypeFromContext(PointerTypeSyntax typeName, TranslationContextWrapper context)
    //    {
    //        context.Error("Pointer types are not suported");
    //        return null;
    //    }

    //    public static Type GetTypeFromContext(NullableTypeSyntax typeName, TranslationContextWrapper context)
    //    {
    //        Type tp = GetTypeFromContext(typeName.ElementType, context);
    //        return typeof (Nullable<>).MakeGenericType(tp);
    //    }

    //    public static Type GetTypeFromContext(string typeName, TranslationContextWrapper context)
    //    {
    //        return context.Context.Assemblies.GetType(typeName, context.Usings, context.Namespace);
    //    }

    //    public static Type InferTypeFromExpression(ExpressionSyntax expression, TranslationContextWrapper context)
    //    {
    //        if (expression is TypeSyntax)
    //        {
    //            return GetTypeFromContext((TypeSyntax) expression, context);
    //        }

    //        if (expression is LiteralExpressionSyntax)
    //            return InferTypeFromExpression((LiteralExpressionSyntax) expression, context);

    //        if (expression is MemberAccessExpressionSyntax)
    //            return InferTypeFromExpression((MemberAccessExpressionSyntax) expression, context);
    //        if (expression is InvocationExpressionSyntax)
    //            return InferTypeFromExpression((InvocationExpressionSyntax) expression, context);
    //        if (expression is ObjectCreationExpressionSyntax)
    //            return InferTypeFromExpression((ObjectCreationExpressionSyntax) expression, context);
    //        if (expression is ThisExpressionSyntax)
    //            return InferTypeFromExpression((ThisExpressionSyntax) expression, context);
    //        if (expression is ElementAccessExpressionSyntax)
    //            return InferTypeFromExpression((ElementAccessExpressionSyntax) expression, context);
    //        if (expression is AnonymousObjectCreationExpressionSyntax)
    //            return InferTypeFromExpression((AnonymousObjectCreationExpressionSyntax) expression, context);
    //        if (expression is BinaryExpressionSyntax)
    //            return InferTypeFromExpression((BinaryExpressionSyntax) expression, context);
    //        if (expression is ArrayCreationExpressionSyntax)
    //            return InferTypeFromExpression((ArrayCreationExpressionSyntax) expression, context);
    //        if (expression is ParenthesizedExpressionSyntax)
    //            return InferTypeFromExpression((ParenthesizedExpressionSyntax) expression, context);
    //        if (expression is ImplicitArrayCreationExpressionSyntax)
    //            return InferTypeFromExpression((ImplicitArrayCreationExpressionSyntax) expression, context);
    //        return null;
    //    }

    //    public static Type InferTypeFromExpression(LiteralExpressionSyntax expression, TranslationContextWrapper context)
    //    {
    //        SyntaxToken literal = expression.Token;
    //        if (literal.RawKind == (int) SyntaxKind.StringLiteralToken)
    //        {
    //            return typeof (string);
    //        }

    //        if (literal.RawKind == (int) SyntaxKind.NumericLiteralToken)
    //        {
    //            string valueString = literal.ValueText;
    //            int resultInt;
    //            if (int.TryParse(valueString, out resultInt)) return typeof (int);
    //            if (valueString.EndsWith("f")) return typeof (float);
    //            return typeof (double);
    //        }

    //        if (literal.RawKind == (int) SyntaxKind.NullKeyword)
    //        {
    //            return null;
    //        }
    //        if (literal.RawKind == (int) SyntaxKind.TrueKeyword || literal.RawKind == (int) SyntaxKind.FalseKeyword)
    //        {
    //            return typeof (bool);
    //        }
    //        return null;
    //    }

    //    public static Type InferTypeFromExpression(MemberAccessExpressionSyntax expression, TranslationContextWrapper context)
    //    {
    //        Type type = InferTypeFromExpression(expression.Expression, context);
    //        if (type == null) return null;
    //        MemberInfo member = GetMember(type, expression.Name.Identifier.ValueText);
    //        if (member == null) return null;

    //        if (member is FieldInfo) return ((FieldInfo) member).FieldType;
    //        if (member is PropertyInfo) return ((PropertyInfo) member).PropertyType;
    //        if (member is MethodInfo) return ((MethodInfo) member).ReturnType; //todo delegates
    //        return null;
    //    }

    //    public static MemberInfo GetMember(Type type, string memberName)
    //    {
    //        MemberInfo[] members = GetMembers(type, memberName);
    //        if (members.Length == 0) return null;
    //        MemberInfo member = members[0];
    //        return member;
    //    }

    //    public static MemberInfo[] GetMembers(Type type, string memberName)
    //    {
    //        return type.GetMember(memberName, MemberTypes.All,
    //                                              BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public |
    //                                              BindingFlags.NonPublic | BindingFlags.SetField |
    //                                              BindingFlags.GetProperty | BindingFlags.SetField |
    //                                              BindingFlags.GetField);
            
    //    }

    //    public static Type InferTypeFromExpression(InvocationExpressionSyntax expression, TranslationContextWrapper context)
    //    {
    //        ExpressionSyntax callee = expression.Expression;
    //        if (callee is IdentifierNameSyntax) //todo this method call or delegate call
    //        {
    //            callee = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
    //                                                          SyntaxFactory.ThisExpression(),(SimpleNameSyntax) callee);
    //        }
    //        if (callee is MemberAccessExpressionSyntax)
    //        {
    //            var memberCallee = callee as MemberAccessExpressionSyntax;
    //            SimpleNameSyntax name = memberCallee.Name;
    //            Type calleeType = InferTypeFromExpression(memberCallee.Expression, context);
    //            if (calleeType == null) return null;

    //            var arguments = new LinkedList<Type>();
    //            foreach (ArgumentSyntax argumentSyntax in expression.ArgumentList.Arguments)
    //            {
    //                arguments.AddLast(InferTypeFromExpression(argumentSyntax.Expression, context));
    //            }
    //            var genericArguments = new LinkedList<Type>();
    //            if (name is GenericNameSyntax)
    //            {
    //                var genericNameSyntax = (GenericNameSyntax) name;
    //                foreach (TypeSyntax typeName in genericNameSyntax.TypeArgumentList.Arguments)
    //                {
    //                    genericArguments.AddLast(GetTypeFromContext(typeName, context));
    //                }
    //            }
    //            var members = GetMembers(calleeType, name.Identifier.ValueText);
    //            if (members==null) return null;
    //            var args = arguments.ToArray();
    //            var methods = members.Where(c => c.MemberType == MemberTypes.Method).Cast<MethodInfo>();
    //            foreach (var memberInfo in methods)
    //            {
    //                var prms = memberInfo.GetParameters();
    //                if (prms.Length!=args.Length) continue;
    //                bool inconsistent = false;
    //                for (int i = 0; i < prms.Length; i++)
    //                {
    //                    if (prms[i].ParameterType!=args[i])
    //                    {
    //                        inconsistent = true;
    //                        break;
    //                    }
    //                }
    //                if (!inconsistent)
    //                {
    //                    return memberInfo.ReturnType;
    //                }
    //            }

    //            return null;
    //        }
    //        return InferTypeFromExpression(expression.Expression, context);
    //    }

    //    public static Type InferTypeFromExpression(ObjectCreationExpressionSyntax expression, TranslationContextWrapper context)
    //    {
    //        return GetTypeFromContext(expression.Type, context);
    //    }

    //    public static Type InferTypeFromExpression(ThisExpressionSyntax expression, TranslationContextWrapper context)
    //    {
    //        return context.CurrentClassContext.Type;
    //    }

    //    public static Type InferTypeFromExpression(ElementAccessExpressionSyntax expression, TranslationContextWrapper context)
    //    {
    //        Type t = InferTypeFromExpression(expression.Expression, context);
    //        if (t == null) return null;
    //        MemberInfo indexer = GetMember(t, "Item");
    //        if (indexer == null) return null;
    //        var idxProp = (PropertyInfo) indexer;
    //        return idxProp.PropertyType;
    //    }

    //    public static Type InferTypeFromExpression(AnonymousObjectCreationExpressionSyntax expression,
    //                                               TranslationContextWrapper context)
    //    {
    //        return typeof (object);
    //    }

    //    public static Type GetTypeFromContext(IdentifierNameSyntax expression, TranslationContextWrapper context)
    //    {
    //        //method names are not passed here
    //        string name = expression.Identifier.ValueText;
    //        Type localVarType = null;
    //        if (context.CurrentClassContext.CurrentFunction != null)
    //        {
    //            var v = context.CurrentClassContext.CurrentFunction.LocalVariables.GetVariable(name);
    //            if (v!=null)
    //            {
    //                localVarType = v.VariableType;
    //            }
    //        }
    //        if (localVarType != null) return localVarType;

    //        Type memberInfoType = null;
    //        MemberInfo member = GetMember(context.CurrentClassContext.Type,name);
    //        if (member != null)
    //        {
    //            if (member is FieldInfo) memberInfoType = ((FieldInfo) member).FieldType;
    //            if (member is PropertyInfo) memberInfoType = ((PropertyInfo) member).PropertyType;
    //        }
    //        if (memberInfoType != null) return memberInfoType;

    //        //static?
    //        Type staticType = GetTypeFromContext(name, context);
    //        if (staticType != null) return staticType;

    //        return null;
    //    }

    //    public static Type InferTypeFromExpression(BinaryExpressionSyntax expression, TranslationContextWrapper context)
    //    {
    //        return null; //todo infer type from binary expressions
    //    }

    //    public static Type InferTypeFromExpression(ArrayCreationExpressionSyntax expression, TranslationContextWrapper context)
    //    {
    //        Type arrType = GetTypeFromContext(expression.Type, context);
    //        return arrType.GetElementType();
    //    }

    //    public static Type InferTypeFromExpression(ParenthesizedExpressionSyntax expression, TranslationContextWrapper context)
    //    {
    //        return InferTypeFromExpression(expression.Expression, context);
    //    }

    //    public static Type InferTypeFromExpression(ImplicitArrayCreationExpressionSyntax expression,
    //                                               TranslationContextWrapper context)
    //    {
    //        if (expression.Initializer.Expressions.Count == 0) return null;
    //        foreach (ExpressionSyntax ex in expression.Initializer.Expressions)
    //        {
    //            Type type = InferTypeFromExpression(ex, context);
    //            if (type != null) return type;
    //        }
    //        return null;
    //    }
    //}
}