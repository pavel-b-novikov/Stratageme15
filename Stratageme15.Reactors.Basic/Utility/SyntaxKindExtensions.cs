using System;
using Microsoft.CodeAnalysis.CSharp;
using Stratageme15.Core.JavascriptCodeDom;

namespace Stratageme15.Reactors.Basic.Utility
{
    public static class SyntaxKindExtensions
    {
        public static Enum ToOperator(this SyntaxKind kind, Type enumType)
        {
            if (typeof (LogicalOperator).IsAssignableFrom(enumType)) return ConvertLogical(kind);
            if (typeof (BitwiseOperator).IsAssignableFrom(enumType)) return ConvertBitWise(kind);
            if (typeof (AssignmentOperator).IsAssignableFrom(enumType)) return ConvertAssignment(kind);
            if (typeof (UnaryOperator).IsAssignableFrom(enumType)) return ConvertUnary(kind);
            if (typeof (MathOperator).IsAssignableFrom(enumType)) return ConvertMath(kind);
            if (typeof (ComparisonOperator).IsAssignableFrom(enumType)) return ConvertComparison(kind);
            if (typeof (LogicalOperator).IsAssignableFrom(enumType)) return ConvertUnary(kind);

            throw new Exception(string.Format("Unknown operator {0}", kind));
        }

        public static BitwiseOperator ConvertBitWise(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.AmpersandToken:
                    return BitwiseOperator.And;
                case SyntaxKind.BarToken:
                    return BitwiseOperator.Or;
                case SyntaxKind.CaretToken:
                    return BitwiseOperator.Xor;
                case SyntaxKind.LessThanLessThanToken:
                    return BitwiseOperator.LeftShift;
                case SyntaxKind.GreaterThanGreaterThanToken:
                    return BitwiseOperator.RightShift;
            }
            throw new Exception(string.Format("Unknown operator {0}", kind));
        }

        public static IndrementDecrementOperator ConvertIndrementDecrement(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusPlusToken:
                    return IndrementDecrementOperator.Increment;
                case SyntaxKind.MinusMinusToken:
                    return IndrementDecrementOperator.Decrement;
            }
            throw new Exception(string.Format("Unknown operator {0}", kind));
        }

        public static AssignmentOperator ConvertAssignment(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.EqualsToken:
                    return AssignmentOperator.Set;
                case SyntaxKind.PlusEqualsToken:
                    return AssignmentOperator.PlusSet;
                case SyntaxKind.MinusEqualsToken:
                    return AssignmentOperator.MinusSet;
                case SyntaxKind.AsteriskEqualsToken:
                    return AssignmentOperator.MulSet;
                case SyntaxKind.SlashEqualsToken:
                    return AssignmentOperator.DivSet;
                case SyntaxKind.LessThanLessThanEqualsToken:
                    return AssignmentOperator.LeftShiftSet;
                case SyntaxKind.GreaterThanGreaterThanEqualsToken:
                    return AssignmentOperator.RightShiftSet;
                case SyntaxKind.AmpersandEqualsToken:
                    return AssignmentOperator.AndSet;
                case SyntaxKind.BarEqualsToken:
                    return AssignmentOperator.OrSet;
                case SyntaxKind.CaretEqualsToken:
                    return AssignmentOperator.XorSet;
            }
            throw new Exception(string.Format("Unknown operator {0}", kind));
        }

        public static UnaryOperator ConvertUnary(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                    return UnaryOperator.Plus;
                case SyntaxKind.MinusToken:
                    return UnaryOperator.Minus;
                case SyntaxKind.BitwiseNotExpression:
                    return UnaryOperator.BitwiseNot;
                case SyntaxKind.LogicalNotExpression:
                    return UnaryOperator.LogicalNot;
                case SyntaxKind.PlusPlusToken:
                    return UnaryOperator.Increment;
                case SyntaxKind.MinusMinusToken:
                    return UnaryOperator.Decrement;
                case SyntaxKind.ExclamationToken:
                    return UnaryOperator.LogicalNot;
            }
            throw new Exception(string.Format("Unknown operator {0}", kind));
        }


        public static MathOperator ConvertMath(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                    return MathOperator.Plus;
                case SyntaxKind.MinusToken:
                    return MathOperator.Minus;
                case SyntaxKind.SlashToken:
                    return MathOperator.Div;
                case SyntaxKind.AsteriskToken:
                    return MathOperator.Mul;
                case SyntaxKind.PercentToken:
                    return MathOperator.Mod;
            }
            throw new Exception(string.Format("Unknown operator {0}", kind));
        }

        public static ComparisonOperator ConvertComparison(this SyntaxKind kind)
        {
            /*
             *  Less,
        More,
        LessOrEqual,
        MoreOrEqual,
        NotEqual,
        Equal,
        TypeEqual,
        TypeNotEqual,
             */
            switch (kind)
            {
                case SyntaxKind.LessThanToken:
                    return ComparisonOperator.Less;
                case SyntaxKind.GreaterThanToken:
                    return ComparisonOperator.More;
                case SyntaxKind.NotEqualsExpression:
                    return ComparisonOperator.NotEqual;
                case SyntaxKind.LessThanEqualsToken:
                    return ComparisonOperator.LessOrEqual;
                case SyntaxKind.GreaterThanEqualsToken:
                    return ComparisonOperator.MoreOrEqual;
                case SyntaxKind.ExclamationEqualsToken:
                    return ComparisonOperator.NotEqual;
                case SyntaxKind.EqualsEqualsToken:
                    return ComparisonOperator.Equal;
            }
            throw new Exception(string.Format("Unknown operator {0}", kind));
        }

        public static LogicalOperator ConvertLogical(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.AmpersandAmpersandToken:
                    return LogicalOperator.And;
                case SyntaxKind.BarBarToken:
                    return LogicalOperator.Or;
            }
            throw new Exception(string.Format("Unknown operator {0}", kind));
        }
    }
}