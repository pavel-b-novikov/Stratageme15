namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Literals.KeywordLiterals
{
    public class ThisKeywordLiteralExpression : KeywordLiteralExpression
    {
        public override string ToString()
        {
            return Literal;
        }

        public override string Literal
        {
            get { return "this"; }
        }
    }
}