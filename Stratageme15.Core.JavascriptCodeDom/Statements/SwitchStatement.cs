using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class SwitchStatement : SyntaxTreeNodeBase, IRootStatement, IStatement
    {
        public ParenthesisExpression SwitchExpression { get; set; }

        public IList<CaseClause> Cases { get; private set; }

        public DefaultClause Default { get; set; }

        public SwitchStatement()
        {
            Cases = new List<CaseClause>();
        }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<SwitchStatement, ParenthesisExpression>(c => c.SwitchExpression, symbol)) return;
           
            if (Is<CaseClause>(symbol))
            {
                Cases.Add((CaseClause) symbol);
                symbol.Parent = this;
                return;
            }

            if (CollectExact<SwitchStatement, DefaultClause>(c => c.Default, symbol)) return;
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (SwitchExpression != null) yield return SwitchExpression;
            if (Cases != null)
            {
                foreach (var caseClause in Cases) yield return caseClause;
            }
            if (Default != null) yield return Default;

        }

        public StatementLabel Label { get; set; }
    }
}