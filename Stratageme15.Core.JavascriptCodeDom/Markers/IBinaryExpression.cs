using Stratageme15.Core.JavascriptCodeDom.Expressions;

namespace Stratageme15.Core.JavascriptCodeDom.Markers
{
    public interface IBinaryExpression
    {
        Expression LeftPart { get; set; }

        Expression RightPart { get; set; }
    }
}
