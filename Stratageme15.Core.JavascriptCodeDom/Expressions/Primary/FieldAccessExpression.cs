using System.Collections.Generic;
using System.Linq;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Primary
{
    public class FieldAccessExpression : PrimaryExpression,IRootStatement,IStatement
    {
        public Expression Accessee { get; set; }

        public IdentExpression Member { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Is<IdentExpression>(symbol))
            {
                if (Accessee==null)
                {
                    Accessee = (Expression) symbol;
                    symbol.Parent = this;
                    return;
                }
                if (Member == null)
                {
                    Member = (IdentExpression)symbol;
                    symbol.Parent = this;
                    return;
                }
            }

            if (CollectExact<FieldAccessExpression,Expression>(c=>c.Accessee,symbol)) return;
            
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            yield return Accessee;
            yield return Member;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}",Accessee,Member);
        }

        public StatementLabel Label { get; set; }
    }
}