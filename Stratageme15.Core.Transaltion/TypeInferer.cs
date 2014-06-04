using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.Transaltion.TranslationContexts;
// ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
namespace Stratageme15.Core.Transaltion
{
    public class AnonymousType
    {
        
    }
    public static class TypeInferer
    {
        private static readonly Dictionary<string, Type> _predefinedTypes = new Dictionary<string, Type>(){
            {"string", typeof(string)}, 
            {"char", typeof(char)},
            {"byte", typeof(byte)},
            {"sbyte", typeof(sbyte)},
            {"ushort", typeof(ushort)},
            {"short", typeof(short)},
            {"uint", typeof(uint)},
            {"int", typeof(int)},
            {"ulong", typeof(ulong)},
            {"long", typeof(long)},
            {"float", typeof(float)},
            {"double", typeof(double)},
            {"decimal", typeof(decimal)},
            {"object", typeof(object)},
            {"bool", typeof(bool)}
            };
        public static bool IsPredefinedNumeric(this Type t)
        {
            return t == typeof (byte)
                   || t == typeof (sbyte)
                   || t == typeof (ushort)
                   || t == typeof (short)
                   || t == typeof (uint)
                   || t == typeof (int)
                   || t == typeof (ulong)
                   || t == typeof (long)
                   || t == typeof (float)
                   || t == typeof (double)
                   || t == typeof (decimal);
        }

        public static LiteralExpressionSyntax GetDefaultValueSyntax(this Type t)
        {
            if ((!t.IsValueType) || (t==typeof(char)))
                return Syntax.LiteralExpression(SyntaxKind.NullLiteralExpression);
            if (IsPredefinedNumeric(t))
            {
                return Syntax.LiteralExpression(SyntaxKind.NumericLiteralExpression,Syntax.Literal(Syntax.TriviaList(),@"0",0,Syntax.TriviaList()));
            }
            if (t == typeof(bool))
            {
                return Syntax.LiteralExpression(SyntaxKind.FalseLiteralExpression);
            }
            throw new Exception("unknown type for default expression");
        }


        public static Type GetTypeFromContext(TypeSyntax typeName, TranslationContext context)
        {
            if (typeName is GenericNameSyntax) return GetTypeFromContext((GenericNameSyntax)typeName, context);
            if (typeName is IdentifierNameSyntax) return GetTypeFromContext((IdentifierNameSyntax)typeName, context);
            if (typeName is AliasQualifiedNameSyntax) return GetTypeFromContext((AliasQualifiedNameSyntax)typeName, context);
            if (typeName is QualifiedNameSyntax) return GetTypeFromContext((QualifiedNameSyntax)typeName, context);
            if (typeName is PredefinedTypeSyntax) return GetTypeFromContext((PredefinedTypeSyntax)typeName, context);
            if (typeName is ArrayTypeSyntax) return GetTypeFromContext((ArrayTypeSyntax)typeName, context);
            if (typeName is PointerTypeSyntax) return GetTypeFromContext((PointerTypeSyntax)typeName, context);
            if (typeName is NullableTypeSyntax) return GetTypeFromContext((NullableTypeSyntax)typeName, context);

            //Well, here we dont actually react OmittedTypeArgumentSyntax 
            //because it's usage is strongly restricted and we will handle it
            //in special order

            throw new Exception("Unknown TypeSintax");
        }

        public static Type GetTypeFromContext(GenericNameSyntax typeName, TranslationContext context)
        {
            var rawtypeName = typeName.Identifier.ValueText;
            var rawType = GetTypeFromContext(rawtypeName, context);
            List<Type> generics = new List<Type>();
            foreach (var arg in typeName.TypeArgumentList.Arguments)
            {
                if (!(arg is OmittedTypeArgumentSyntax))
                {
                    generics.Add(GetTypeFromContext(arg, context));
                }
            }
            
            if (generics.Count == 0) return rawType;
            return rawType.MakeGenericType(generics.ToArray());
        }

        public static Type GetTypeFromContext(IdentifierNameSyntax typeName, TranslationContext context)
        {
            return GetTypeFromContext(typeName.Identifier.ValueText, context);
        }

        public static Type GetTypeFromContext(AliasQualifiedNameSyntax typeName, TranslationContext context)
        {
            throw new Exception("I've just didn't understand when AliasQualifiedNameSyntax actually met");
        }
        public static Type GetTypeFromContext(QualifiedNameSyntax typeName, TranslationContext context)
        {
            string fullTypeName = typeName.ToFullString();
            return GetTypeFromContext(fullTypeName, context);
        }
        public static Type GetTypeFromContext(PredefinedTypeSyntax typeName, TranslationContext context)
        {
            var predefinedName = typeName.Keyword.ValueText;
            return _predefinedTypes[predefinedName];
        }
        public static Type GetTypeFromContext(ArrayTypeSyntax typeName, TranslationContext context)
        {
            Type elementType = GetTypeFromContext(typeName.ElementType, context);
            Type arrayedType = elementType;
            foreach (var arrayRankSpecifierSyntax in typeName.RankSpecifiers)
            {
                arrayedType = arrayedType.MakeArrayType(arrayRankSpecifierSyntax.Rank);
            }
            return arrayedType;
        }
        public static Type GetTypeFromContext(PointerTypeSyntax typeName, TranslationContext context)
        {
            throw new Exception("Pointer types are not suported");
        }
        public static Type GetTypeFromContext(NullableTypeSyntax typeName, TranslationContext context)
        {
            var tp = GetTypeFromContext(typeName.ElementType, context);
            return typeof(Nullable<>).MakeGenericType(tp);
        }

        public static Type GetTypeFromContext(string typeName, TranslationContext context)
        {
            return context.Assemblies.GetType(typeName, context.Usings, context.Namespace);
        }

        public static Type InferTypeFromExpression(ExpressionSyntax expression, TranslationContext context)
        {

            if (expression is LiteralExpressionSyntax) return InferTypeFromExpression((LiteralExpressionSyntax) expression, context);

            if (expression is MemberAccessExpressionSyntax) return InferTypeFromExpression((MemberAccessExpressionSyntax)expression, context);
            if (expression is InvocationExpressionSyntax) return InferTypeFromExpression((InvocationExpressionSyntax)expression, context);
            if (expression is ObjectCreationExpressionSyntax) return InferTypeFromExpression((ObjectCreationExpressionSyntax)expression, context);
            if (expression is ThisExpressionSyntax) return InferTypeFromExpression((ThisExpressionSyntax)expression, context);
            if (expression is ElementAccessExpressionSyntax) return InferTypeFromExpression((ElementAccessExpressionSyntax)expression, context);
            if (expression is AnonymousObjectCreationExpressionSyntax) return InferTypeFromExpression((AnonymousObjectCreationExpressionSyntax)expression, context);
            if (expression is IdentifierNameSyntax) return InferTypeFromExpression((IdentifierNameSyntax)expression, context);
            if (expression is BinaryExpressionSyntax) return InferTypeFromExpression((BinaryExpressionSyntax)expression, context);
            if (expression is ArrayCreationExpressionSyntax) return InferTypeFromExpression((ArrayCreationExpressionSyntax)expression, context);
            if (expression is ParenthesizedExpressionSyntax) return InferTypeFromExpression((ParenthesizedExpressionSyntax)expression, context);
            if (expression is ImplicitArrayCreationExpressionSyntax) return InferTypeFromExpression((ImplicitArrayCreationExpressionSyntax)expression, context);
            return null;
        }

        public static Type InferTypeFromExpression(LiteralExpressionSyntax expression, TranslationContext context)
        {
            var literal = expression.Token;
            if (literal.Kind == SyntaxKind.StringLiteralToken)
            {
                return typeof(string);
            }

            if (literal.Kind == SyntaxKind.NumericLiteralToken)
            {
                var valueString = literal.ValueText;
                int resultInt;
                if (int.TryParse(valueString, out resultInt)) return typeof (int);
                if (valueString.EndsWith("f")) return typeof (float);
                return typeof (double);
            }

            if (literal.Kind == SyntaxKind.NullKeyword)
            {
                return null;
            }
            if (literal.Kind == SyntaxKind.TrueKeyword || literal.Kind == SyntaxKind.FalseKeyword)
            {
                return typeof(bool);
            }
            return null;
        }

        public static Type InferTypeFromExpression(MemberAccessExpressionSyntax expression, TranslationContext context)
        {
            var type = InferTypeFromExpression(expression.Expression,context);
            if (type == null) return null;
            var member = GetMember(type, expression.Name.Identifier.ValueText);
            if (member == null) return null;

            if (member is FieldInfo) return ((FieldInfo)member).FieldType;
            if (member is PropertyInfo) return ((PropertyInfo) member).PropertyType;
            if (member is MethodInfo) return ((MethodInfo) member).ReturnType; //todo delegates
            return null;
        }

        private static MemberInfo GetMember(Type type, string memberName)
        {
            var members = type.GetMember(memberName);
            if (members.Length == 0) return null;
            var member = members[0];
            return member;
        }

        public static Type InferTypeFromExpression(InvocationExpressionSyntax expression, TranslationContext context)
        {
            var callee = expression.Expression;
            if (callee is MemberAccessExpressionSyntax)
            {
                var memberCallee = callee as MemberAccessExpressionSyntax;
                var name = memberCallee.Name;
                var calleeType = InferTypeFromExpression(memberCallee.Expression, context);
                if (calleeType == null) return null;

                LinkedList<Type> arguments = new LinkedList<Type>();
                foreach (var argumentSyntax in expression.ArgumentList.Arguments)
                {
                    arguments.AddLast(InferTypeFromExpression(argumentSyntax.Expression, context));
                }
                LinkedList<Type> genericArguments = new LinkedList<Type>();
                if (name is GenericNameSyntax)
                {
                    var genericNameSyntax = (GenericNameSyntax) name;
                    foreach (var typeName in genericNameSyntax.TypeArgumentList.Arguments)
                    {
                        genericArguments.AddLast(GetTypeFromContext(typeName, context));
                    }
                }
                var method = calleeType.GetMethod(name.Identifier.ValueText, arguments.ToArray());
                if (method == null) return null;
                return method.ReturnType;
            }
            return InferTypeFromExpression(expression.Expression, context);
        }

        public static Type InferTypeFromExpression(ObjectCreationExpressionSyntax expression, TranslationContext context)
        {
            return GetTypeFromContext(expression.Type,context);
        }

        public static Type InferTypeFromExpression(ThisExpressionSyntax expression, TranslationContext context)
        {
            return context.CurrentClassContext.Type;
        }

        public static Type InferTypeFromExpression(ElementAccessExpressionSyntax expression, TranslationContext context)
        {
            var t = InferTypeFromExpression(expression.Expression, context);
            if (t == null) return null;
            var indexer = GetMember(t, "Item");
            if (indexer == null) return null;
            var idxProp = (PropertyInfo) indexer;
            return idxProp.PropertyType;
        }

        public static Type InferTypeFromExpression(AnonymousObjectCreationExpressionSyntax expression, TranslationContext context)
        {
            return typeof(object);
        }

        public static Type InferTypeFromExpression(IdentifierNameSyntax expression, TranslationContext context)
        {
            string name = expression.Identifier.ValueText;
            Type localVarType = null;
            if (context.CurrentClassContext.CurrentFunction != null)
            {
                foreach (var variable in context.CurrentClassContext.CurrentFunction.LocalVariables.AllVariables)
                {
                    if (variable.VariableName == name)
                    {
                        localVarType = variable.VariableType;
                        break;
                    }
                }
            }
            if (localVarType != null) return localVarType;

            Type memberInfoType = null;
            var member = context.CurrentClassContext.Type.GetMembers(
                BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.SetField | BindingFlags.GetField |
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
                ).FirstOrDefault(c=>c.Name==name);
            if (member!=null)
            {
                if (member is FieldInfo) memberInfoType = ((FieldInfo)member).FieldType;
                if (member is PropertyInfo) memberInfoType = ((PropertyInfo)member).PropertyType;
            }
            if (memberInfoType != null) return memberInfoType;

            //static?
            var staticType = GetTypeFromContext(name, context);
            if (staticType != null) return staticType;

            return null;
        }

        public static Type InferTypeFromExpression(BinaryExpressionSyntax expression, TranslationContext context)
        {
            return null;
        }
        public static Type InferTypeFromExpression(ArrayCreationExpressionSyntax expression, TranslationContext context)
        {
            var arrType = GetTypeFromContext(expression.Type, context);
            return arrType.GetElementType();
        }

        public static Type InferTypeFromExpression(ParenthesizedExpressionSyntax expression, TranslationContext context)
        {
            return InferTypeFromExpression(expression.Expression,context);
        }
        public static Type InferTypeFromExpression(ImplicitArrayCreationExpressionSyntax expression, TranslationContext context)
        {
            if (expression.Initializer.Expressions.Count == 0) return null;
            foreach (var ex in expression.Initializer.Expressions)
            {
                var type = InferTypeFromExpression(ex,context);
                if (type != null) return type;
            }
            return null;
        }
    }
}
