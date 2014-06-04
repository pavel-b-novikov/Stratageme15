namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Literals.KeywordLiterals
{
    public class FalseBooleanKeywordLiteralExpression : BooleanKeywordLiteralExpression
    {
        public override string ToString()
        {
            return Literal;
        }

        public override string Literal
        {
            get { return "false"; }
        }
    }
}