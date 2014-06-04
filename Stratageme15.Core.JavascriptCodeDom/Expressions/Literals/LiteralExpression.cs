using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Literals
{
    public abstract class LiteralExpression : PrimaryExpression
    {
        public abstract string Literal { get; }
    }
}