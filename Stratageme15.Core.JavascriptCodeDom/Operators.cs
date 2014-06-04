namespace Stratageme15.Core.JavascriptCodeDom
{
    public enum IndrementDecrementOperator
    {
        Increment,
        Decrement
    }
   
    public enum AssignmentOperator
    {
        Set,
        PlusSet,
        MinusSet,
        MulSet,
        DivSet,
        ModSet,
        LeftShiftSet,
        RightShiftSet,
        UnnsignedRightShiftSet,
        OrSet,
        AndSet,
        XorSet
    }

    public enum UnaryOperator
    {
        Plus,
        Minus,
        BitwiseNot,
        LogicalNot,
        Increment,
        Decrement,
    }

    public enum MathOperator
    {
        Plus,
        Minus,
        Div,
        Mul,
        Mod,
    }

    public enum LogicalOperator
    {
        And,
        Or,
        In,
        Instanceof
    }

    public enum BitwiseOperator
    {
        And,
        Or,
        Xor,
        LeftShift,
        RightShift,
        UnsignedRightShift
    }

    public enum ComparisonOperator
    {
        Less,
        More,
        LessOrEqual,
        MoreOrEqual,
        NotEqual,
        Equal,
        TypeEqual,
        TypeNotEqual,
    }
}