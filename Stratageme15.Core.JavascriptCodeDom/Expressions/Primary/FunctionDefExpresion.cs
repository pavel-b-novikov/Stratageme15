using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Primary
{
    public class FunctionDefExpression : PrimaryExpression, IRootStatement, IStatement
    {
        public IdentExpression Name { get; set; }

        public FormalParametersList Parameters { get; set; }

        public CodeBlock Code { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<FunctionDefExpression,IdentExpression>(c=>c.Name,symbol)) return;
            if (CollectExact<FunctionDefExpression, FormalParametersList>(c => c.Parameters, symbol)) return;
            if (CollectExact<FunctionDefExpression, CodeBlock>(c => c.Code, symbol)) return;
            
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (Name != null) yield return Name;
            if (Parameters != null) yield return Parameters;
            if (Code != null) yield return Code;
        }

        public override string ToString()
        {
            return string.Format("function {0}", Name);
        }

        public StatementLabel Label { get; set; }
    }
}