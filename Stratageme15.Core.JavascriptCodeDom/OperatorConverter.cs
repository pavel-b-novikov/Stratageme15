using System.Collections.Generic;

namespace Stratageme15.Core.JavascriptCodeDom
{
    internal static class DictionaryExtensions
    {
        internal static TKey GetKeyByValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value)
        {
            foreach (var kv in dict)
            {
                if (kv.Value.Equals(value)) return kv.Key;
            }
            throw new KeyNotFoundException(value.ToString());
        }
    }
    public class OperatorConverter
    {
        private static Dictionary<string, AssignmentOperator> AssignmentOperator =
            new Dictionary<string, AssignmentOperator>
                    {
                        { "=",JavascriptCodeDom.AssignmentOperator.Set},
                        { "+=", JavascriptCodeDom.AssignmentOperator.PlusSet},
                        { "-=", JavascriptCodeDom.AssignmentOperator.MinusSet},
                        { "*=", JavascriptCodeDom.AssignmentOperator.MulSet},
                        { "/=", JavascriptCodeDom.AssignmentOperator.DivSet},
                        { "<<=", JavascriptCodeDom.AssignmentOperator.LeftShiftSet},
                        { ">>=", JavascriptCodeDom.AssignmentOperator.RightShiftSet},
                        { ">>>=", JavascriptCodeDom.AssignmentOperator.UnnsignedRightShiftSet},
                        { "%=", JavascriptCodeDom.AssignmentOperator.ModSet},
                        { "|=", JavascriptCodeDom.AssignmentOperator.OrSet},
                        { "&=", JavascriptCodeDom.AssignmentOperator.AndSet},
                        { "^=", JavascriptCodeDom.AssignmentOperator.XorSet}

                    };

        private static Dictionary<string, UnaryOperator> UnaryOperator =
            new Dictionary<string, UnaryOperator>
                    {
                        { "+", JavascriptCodeDom.UnaryOperator.Plus},
                        { "-", JavascriptCodeDom.UnaryOperator.Minus},
                        { "~", JavascriptCodeDom.UnaryOperator.BitwiseNot},
                        { "!", JavascriptCodeDom.UnaryOperator.LogicalNot},
                        { "++", JavascriptCodeDom.UnaryOperator.Increment},
                        { "--", JavascriptCodeDom.UnaryOperator.Decrement}
                    };
        private static Dictionary<string, IndrementDecrementOperator> IndrementDecrementOperator =
            new Dictionary<string, IndrementDecrementOperator>
                {
                    {"++",JavascriptCodeDom.IndrementDecrementOperator.Increment},
                    {"--",JavascriptCodeDom.IndrementDecrementOperator.Decrement}
                }
            ;
        private static Dictionary<string, MathOperator> MathOperator =
            new Dictionary<string, MathOperator>
                    {
                        { "+", JavascriptCodeDom.MathOperator.Plus},
                        { "-", JavascriptCodeDom.MathOperator.Minus},
                        { "/", JavascriptCodeDom.MathOperator.Div},
                        { "*", JavascriptCodeDom.MathOperator.Mul},
                        { "%", JavascriptCodeDom.MathOperator.Mod}
                    };

        private static Dictionary<string, LogicalOperator> LogicalOperator =
            new Dictionary<string, LogicalOperator>
                    {
                        {"||",JavascriptCodeDom.LogicalOperator.Or},
                        {"&&",JavascriptCodeDom.LogicalOperator.And},
                        {"in",JavascriptCodeDom.LogicalOperator.In},
                        {"instanceof",JavascriptCodeDom.LogicalOperator.Instanceof}
                    };

        private static Dictionary<string, BitwiseOperator> BitwiseOperator =
            new Dictionary<string, BitwiseOperator>
                    {
                        {"&",JavascriptCodeDom.BitwiseOperator.And},
                        {"|",JavascriptCodeDom.BitwiseOperator.Or},
                        {"^",JavascriptCodeDom.BitwiseOperator.Xor},
                        {"<<",JavascriptCodeDom.BitwiseOperator.LeftShift},
                        {">>",JavascriptCodeDom.BitwiseOperator.RightShift},
                        {">>>",JavascriptCodeDom.BitwiseOperator.UnsignedRightShift}
                    };

        private static Dictionary<string, ComparisonOperator> ComparisonOperator =
            new Dictionary<string, ComparisonOperator>
                    {
                        { "<", JavascriptCodeDom.ComparisonOperator.Less},
                        { ">", JavascriptCodeDom.ComparisonOperator.More},
                        { "<=", JavascriptCodeDom.ComparisonOperator.LessOrEqual},
                        { ">=", JavascriptCodeDom.ComparisonOperator.MoreOrEqual},
                        { "!=", JavascriptCodeDom.ComparisonOperator.NotEqual},
                        { "==", JavascriptCodeDom.ComparisonOperator.Equal},
                        { "===", JavascriptCodeDom.ComparisonOperator.TypeEqual},
                        { "!==", JavascriptCodeDom.ComparisonOperator.TypeNotEqual}
                    };

        public static string OperatorString(BitwiseOperator op)
        {
            return BitwiseOperator.GetKeyByValue(op);
        }

        public static string OperatorString(ComparisonOperator op)
        {
            return ComparisonOperator.GetKeyByValue(op);
        }

        public static string OperatorString(LogicalOperator op)
        {
            return LogicalOperator.GetKeyByValue(op);
        }

        public static string OperatorString(MathOperator op)
        {
            return MathOperator.GetKeyByValue(op);
        }

        public static string OperatorString(UnaryOperator op)
        {
            return UnaryOperator.GetKeyByValue(op);
        }

        public static string OperatorString(AssignmentOperator op)
        {
            return AssignmentOperator.GetKeyByValue(op);
        }

        public static string OperatorString(IndrementDecrementOperator op)
        {
            return IndrementDecrementOperator.GetKeyByValue(op);
        }

        public static BitwiseOperator GetBitwiseOperator(string op)
        {
            return BitwiseOperator[op];
        }
        public static AssignmentOperator GetAssignmentOperator(string op)
        {
            return AssignmentOperator[op];
        }
        public static UnaryOperator GetUnaryOperator(string op)
        {
            return UnaryOperator[op];
        }
        public static MathOperator GetMathOperator(string op)
        {
            return MathOperator[op];
        }
        public static LogicalOperator GetLogicalOperator(string op)
        {
            return LogicalOperator[op];
        }
        public static ComparisonOperator GetComparisonOperator(string op)
        {
            return ComparisonOperator[op];
        }

        public static IndrementDecrementOperator GetIndrementDecrementOperator(string op)
        {
            return IndrementDecrementOperator[op];
        }
    }
}
