using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Primary
{
    public class IdentExpression : PrimaryExpression, IDictionaryKey
    {
        public string Identifier { get; set; }

        public IdentExpression(string identifier)
        {
            Identifier = identifier;
        }

        public override string ToString()
        {
            return string.Format("{0}", Identifier);
        }
    }
}