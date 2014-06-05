using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions
{
    public class SequenceExpression : Expression, IRootStatement,IStatement
    {
        public List<Expression> Sequence { get; set; }

        public SequenceExpression()
        {
            Sequence = new List<Expression>();
        }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Is<Expression>(symbol))
            {
                symbol.Role = "SequencedStatement";
                Sequence.Add((Expression)symbol);
                symbol.Parent = this;
                return;
            }
            if (Is<CallStatement>(symbol))
            {
                var cs = (CallStatement) symbol;
                cs.CallExpression.Parent = this;
                cs.CallExpression.Role = "SequencedStatement";
                Sequence.Add(cs.CallExpression);
                symbol.Parent = this;
                return;
            }
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            return Sequence;
        }

        public StatementLabel Label { get; set; }
    }
}
