using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class VarLetModifier : SyntaxTreeNodeBase
    {
        public bool IsVar { get; set; }

        public bool IsLet { get; set; }

        public VarLetModifier(string s)
        {
            if (s == "var") IsVar = true;
            if (s == "let") IsLet = true;
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            return Empty;
        }
    }
}
