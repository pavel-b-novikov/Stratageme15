using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class ForInStatementVariableDeclaration : SyntaxTreeNodeBase
    {
        public IdentExpression Identifier { get; set; }
        public VarLetModifier Modifier { get; set; }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            yield return Modifier;
            yield return Identifier;

        }
    }
}
