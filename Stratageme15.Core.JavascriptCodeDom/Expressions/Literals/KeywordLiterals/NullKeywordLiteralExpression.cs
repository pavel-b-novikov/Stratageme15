using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Literals.KeywordLiterals
{
    public class NullKeywordLiteralExpression : KeywordLiteralExpression, IDictionaryKey
    {
        public override string ToString()
        {
            return Literal;
        }

        public override string Literal
        {
            get { return "null"; }
        }
    }
}