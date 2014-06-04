
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.JavascriptCodeDom.Markers
{
    public interface IStatement
    {
        StatementLabel Label { get; set; }
    }

    public interface IRootStatement : IRootCodeElement , IStatement
    {
    }
}