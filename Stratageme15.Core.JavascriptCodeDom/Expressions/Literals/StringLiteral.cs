using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Literals
{
    public class StringLiteral : LiteralExpression, IDictionaryKey
    {
        public bool IsQuoted { get; private set; }

        public StringLiteral(string s)
        {
            String = s;
            IsQuoted = true;
        }

        public StringLiteral(string s,bool isQuoted)
        {
            IsQuoted = isQuoted;
            String = s;
            
        }

        public string String { get; set; }

        public override string ToString()
        {
            return string.Format("<<STRING>>{0}", String);
        }

        public override string Literal
        {
            get { return String; }
        }
    }
}