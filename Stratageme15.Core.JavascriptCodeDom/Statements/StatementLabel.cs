using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class StatementLabel : SyntaxTreeNodeBase
    {
        public StatementLabel(string val)
        {
            LabelName = new IdentExpression(val);
            LabelName.Parent = this;
        }

        public IdentExpression LabelName { get; set; }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            return Empty;
        }
    }
}
