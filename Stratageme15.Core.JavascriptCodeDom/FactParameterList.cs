using System.Collections.Generic;
using System.Linq;
using Stratageme15.Core.JavascriptCodeDom.Expressions;

namespace Stratageme15.Core.JavascriptCodeDom
{
    public class FactParameterList : SyntaxTreeNodeBase
    {
        public IList<Expression> Arguments { get; set; }

        public FactParameterList()
        {
            Arguments = new List<Expression>();
        }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Is<Expression>(symbol))
            {
                Arguments.Add((Expression) symbol);
                return;
            }
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            return Arguments;
        }

        public override string ToString()
        {
            return string.Join(", ", Arguments.Select(c => c.ToString()));
        }
    }
}