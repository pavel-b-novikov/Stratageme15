namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Literals
{
    public class RegexLiteral : LiteralExpression
    {
        public string RegexString { get; private set; }

        public RegexLiteral(string regexString)
        {
            RegexString = regexString;
        }
        public override string ToString()
        {
            return string.Format("®{0}", Literal);
        }

        public override string Literal
        {
            get { return RegexString;  }
        }
    }
}