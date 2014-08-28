using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Binary;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.TranslationContexts;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class BinaryExpressionSyntaxReactor : ExpressionReactorBase<BinaryExpressionSyntax, Expression>
    {
        public override Expression TranslateNodeInner(BinaryExpressionSyntax node, TranslationContext context,
                                                      TranslationResult res)
        {
            res.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            switch (node.CSharpKind())
            {
                case SyntaxKind.BitwiseAndExpression:
                case SyntaxKind.BitwiseOrExpression:
                case SyntaxKind.LeftShiftExpression:
                case SyntaxKind.RightShiftExpression:
                case SyntaxKind.ExclusiveOrExpression:
                    return ConstructBinaryExpression<BitwiseBinaryExpression, BitwiseOperator>(node);
                case SyntaxKind.LogicalOrExpression:
                case SyntaxKind.LogicalAndExpression:
                    return ConstructBinaryExpression<LogicalBinaryExpression, LogicalOperator>(node);
                case SyntaxKind.LessThanExpression:
                case SyntaxKind.GreaterThanExpression:
                case SyntaxKind.LessThanOrEqualExpression:
                case SyntaxKind.GreaterThanOrEqualExpression:
                case SyntaxKind.EqualsExpression:
                case SyntaxKind.NotEqualsExpression:
                    return ConstructBinaryExpression<ComparisonBinaryExpression, ComparisonOperator>(node);
                case SyntaxKind.AddExpression:
                case SyntaxKind.SubtractExpression:
                case SyntaxKind.DivideExpression:
                case SyntaxKind.MultiplyExpression:
                case SyntaxKind.ModuloExpression:
                    return ConstructBinaryExpression<MathBinaryExpression, MathOperator>(node);
                case SyntaxKind.SimpleAssignmentExpression:
                case SyntaxKind.AddAssignmentExpression:
                case SyntaxKind.SubtractAssignmentExpression:
                case SyntaxKind.MultiplyAssignmentExpression:
                case SyntaxKind.DivideAssignmentExpression:
                case SyntaxKind.ModuloAssignmentExpression:
                case SyntaxKind.OrAssignmentExpression:
                case SyntaxKind.ExclusiveOrAssignmentExpression:
                case SyntaxKind.AndAssignmentExpression:
                case SyntaxKind.LeftShiftAssignmentExpression:
                case SyntaxKind.RightShiftAssignmentExpression:
                    return ConstructAssignmentExpression(node, context, res);
            }
            throw new Exception(string.Format("Expression of kind {0} is not supported yet", node.CSharpKind()));
        }

        private TExpression ConstructBinaryExpression<TExpression, TOperator>(BinaryExpressionSyntax node)
            where TExpression : BinaryExpression<TOperator>
            where TOperator : struct
        {
            SyntaxKind kind = node.OperatorToken.CSharpKind();
            var result = Activator.CreateInstance<TExpression>();

            Type enumType = typeof (TOperator);
            if (typeof (LogicalOperator).IsAssignableFrom(enumType)) result.CollectOperator(kind.ConvertLogical());
            if (typeof (BitwiseOperator).IsAssignableFrom(enumType)) result.CollectOperator(kind.ConvertBitWise());
            if (typeof (AssignmentOperator).IsAssignableFrom(enumType))
                result.CollectOperator(kind.ConvertAssignment());
            if (typeof (MathOperator).IsAssignableFrom(enumType)) result.CollectOperator(kind.ConvertMath());
            if (typeof (ComparisonOperator).IsAssignableFrom(enumType))
                result.CollectOperator(kind.ConvertComparison());

            return result;
        }

        public AssignmentBinaryExpression ConstructAssignmentExpression(BinaryExpressionSyntax node,
                                                                        TranslationContext context,
                                                                        TranslationResult res)
        {
            bool isSetterRequired = false;
            var ident = node.Left as IdentifierNameSyntax;
            var mae = node.Left as MemberAccessExpressionSyntax;
            string propName = null;
            if (ident != null)
            {
                propName = ident.Identifier.ValueText;
                if (context.IsProperty(propName))
                {
                    isSetterRequired = true;
                }
            }

            if (mae != null)
            {
                Type type = TypeInferer.InferTypeFromExpression(mae.Expression, context);
                propName = mae.Name.Identifier.ValueText;
                if (type.IsProperty(propName))
                {
                    isSetterRequired = true;
                }
            }
            if (isSetterRequired)
            {
                res.PrepareForManualPush(context);
                ExpressionSyntax baseExpression = null;
                if (ident != null) baseExpression = SyntaxFactory.ThisExpression();
                if (mae != null)
                {
                    baseExpression = mae.Expression;
                }
                string setter = string.Format("set{0}", propName);
                MemberAccessExpressionSyntax callMember =
                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, baseExpression,
                                                         SyntaxFactory.IdentifierName(setter));

                ExpressionSyntax argument = null;
                switch (node.CSharpKind())
                {
                    case SyntaxKind.SimpleAssignmentExpression:
                        argument = node.Right;
                        break;
                    case SyntaxKind.AddAssignmentExpression:
                        argument = SyntaxFactory.BinaryExpression(SyntaxKind.AddExpression, node.Left, node.Right);
                        break;
                    case SyntaxKind.SubtractAssignmentExpression:
                        argument = SyntaxFactory.BinaryExpression(SyntaxKind.SubtractExpression, node.Left, node.Right);
                        break;
                    case SyntaxKind.MultiplyAssignmentExpression:
                        argument = SyntaxFactory.BinaryExpression(SyntaxKind.MultiplyExpression, node.Left, node.Right);
                        break;
                    case SyntaxKind.DivideAssignmentExpression:
                        argument = SyntaxFactory.BinaryExpression(SyntaxKind.DivideExpression, node.Left, node.Right);
                        break;
                    case SyntaxKind.ModuloAssignmentExpression:
                        argument = SyntaxFactory.BinaryExpression(SyntaxKind.ModuloExpression, node.Left, node.Right);
                        break;
                    case SyntaxKind.OrAssignmentExpression:
                        argument = SyntaxFactory.BinaryExpression(SyntaxKind.BitwiseOrExpression, node.Left, node.Right);
                        break;
                    case SyntaxKind.ExclusiveOrAssignmentExpression:
                        argument = SyntaxFactory.BinaryExpression(SyntaxKind.ExclusiveOrExpression, node.Left,
                                                                  node.Right);
                        break;
                    case SyntaxKind.AndAssignmentExpression:
                        argument = SyntaxFactory.BinaryExpression(SyntaxKind.BitwiseAndExpression, node.Left, node.Right);
                        break;
                    case SyntaxKind.LeftShiftAssignmentExpression:
                        argument = SyntaxFactory.BinaryExpression(SyntaxKind.LeftShiftExpression, node.Left, node.Right);
                        break;
                    case SyntaxKind.RightShiftAssignmentExpression:
                        argument = SyntaxFactory.BinaryExpression(SyntaxKind.RightShiftExpression, node.Left, node.Right);
                        break;
                }
                ArgumentSyntax arg = SyntaxFactory.Argument(argument);
                InvocationExpressionSyntax call = SyntaxFactory.InvocationExpression(callMember,
                                                                                     SyntaxFactory.ArgumentList(
                                                                                         SyntaxFactory.SeparatedList(
                                                                                             new[] {arg})));

                context.TranslationStack.Push(call);
                return null;
            }
            return ConstructBinaryExpression<AssignmentBinaryExpression, AssignmentOperator>(node);
        }
    }
}