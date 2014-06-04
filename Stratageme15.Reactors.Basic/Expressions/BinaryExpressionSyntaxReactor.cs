using System;
using Roslyn.Compilers.CSharp;
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
        public override Expression TranslateNodeInner(BinaryExpressionSyntax node, TranslationContext context, TranslationResult res)
        {
            res.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            switch (node.Kind)
            {
                    case SyntaxKind.BitwiseAndExpression:
                    case SyntaxKind.BitwiseOrExpression:
                    case SyntaxKind.LeftShiftExpression:
                    case SyntaxKind.RightShiftExpression:
                    case SyntaxKind.ExclusiveOrExpression:
                        return ConstructBinaryExpression<BitwiseBinaryExpression,BitwiseOperator>(node);
                    case SyntaxKind.LogicalOrExpression:
                    case SyntaxKind.LogicalAndExpression:
                        return ConstructBinaryExpression<LogicalBinaryExpression,LogicalOperator>(node);
                    case SyntaxKind.LessThanExpression:
                    case SyntaxKind.GreaterThanExpression:
                    case SyntaxKind.LessThanOrEqualExpression:
                    case SyntaxKind.GreaterThanOrEqualExpression:
                    case SyntaxKind.EqualsExpression:
                        return ConstructBinaryExpression<ComparisonBinaryExpression, ComparisonOperator>(node);
                    case SyntaxKind.AddExpression:
                    case SyntaxKind.SubtractExpression:
                    case SyntaxKind.DivideExpression:
                    case SyntaxKind.MultiplyExpression:
                    case SyntaxKind.ModuloExpression:
                        return ConstructBinaryExpression<MathBinaryExpression,MathOperator>(node);
                    case SyntaxKind.AssignExpression:    
                    case SyntaxKind.AddAssignExpression:
                    case SyntaxKind.SubtractAssignExpression:
                    case SyntaxKind.MultiplyAssignExpression:
                    case SyntaxKind.DivideAssignExpression:
                    case SyntaxKind.ModuloAssignExpression:
                    case SyntaxKind.OrAssignExpression:
                    case SyntaxKind.ExclusiveOrAssignExpression:
                    case SyntaxKind.AndAssignExpression:
                    case SyntaxKind.LeftShiftAssignExpression:
                    case SyntaxKind.RightShiftAssignExpression:
                        return ConstructBinaryExpression<AssignmentBinaryExpression, AssignmentOperator>(node);

            }
            throw new Exception(string.Format("Expression of kind {0} is not supported yet",node.Kind));
        }

        private TExpression ConstructBinaryExpression<TExpression,TOperator>(BinaryExpressionSyntax node) 
            where TExpression:BinaryExpression<TOperator>
            where TOperator : struct 
        {
            var kind = node.OperatorToken.Kind;
            var result = Activator.CreateInstance<TExpression>();

            Type enumType = typeof (TOperator);
            if (typeof(LogicalOperator).IsAssignableFrom(enumType)) result.CollectOperator(kind.ConvertLogical());
            if (typeof(BitwiseOperator).IsAssignableFrom(enumType)) result.CollectOperator(kind.ConvertBitWise());
            if (typeof(AssignmentOperator).IsAssignableFrom(enumType)) result.CollectOperator(kind.ConvertAssignment());
            if (typeof(MathOperator).IsAssignableFrom(enumType)) result.CollectOperator(kind.ConvertMath());
            if (typeof(ComparisonOperator).IsAssignableFrom(enumType)) result.CollectOperator(kind.ConvertComparison());
            if (typeof(LogicalOperator).IsAssignableFrom(enumType)) result.CollectOperator(kind.ConvertLogical());
            
            return result;
        }
    }
}
