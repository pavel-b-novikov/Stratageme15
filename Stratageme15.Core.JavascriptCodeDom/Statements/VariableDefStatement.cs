using System;
using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Exceptions;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class VariableDefStatement : SyntaxTreeNodeBase, IRootStatement
    {
        public IList<Tuple<IdentExpression, AssignmentOperator, Expression>> Variables { get; set; }

        private readonly IList<IdentExpression> _pendingIdents;

        public VariableDefStatement()
        {
            _pendingIdents = new List<IdentExpression>();
            Variables = new List<Tuple<IdentExpression, AssignmentOperator, Expression>>();
        }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (symbol is IdentExpression)
            {
                _pendingIdents.Add((IdentExpression) symbol);
                Variables.Add(new Tuple<IdentExpression, AssignmentOperator, Expression>((IdentExpression) symbol,AssignmentOperator.Set,null));
                return;
            }

            if (symbol is AssignmentStatement)
            {
                var a = (AssignmentStatement) symbol;
                Variables.Add(new Tuple<IdentExpression, AssignmentOperator, Expression>(a.Identifier,a.Operator,a.AssignmentExpression));
                return;
            }

            if (symbol is Expression)
            {
                var a = (Expression) symbol;
                var tuple = Variables[Variables.Count - 1];
                var t = new Tuple<IdentExpression, AssignmentOperator, Expression>(tuple.Item1, tuple.Item2, a);
                Variables.Remove(tuple);
                Variables.Add(t);
                return;
            }
            
            throw new UnexpectedException(symbol,this);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            foreach (var variable in Variables)
            {
                if (variable.Item1!=null) yield return variable.Item1;
                if (variable.Item3 != null) yield return variable.Item3;
            }
        }

        public StatementLabel Label { get; set; }

        public override string ToString()
        {
            return "var ...";
        }
    }
}