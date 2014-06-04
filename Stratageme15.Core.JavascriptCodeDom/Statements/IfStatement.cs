using System;
using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class IfStatement : SyntaxTreeNodeBase, IRootStatement, IStatement
    {
        public IfStatement()
        {
            ElseIfs = new List<Tuple<ParenthesisExpression, CodeBlock>>();
        }

        public ParenthesisExpression Condition { get; set; }

        public CodeBlock TrueCodeBlock { get; set; }

        public CodeBlock ElseCodeBlock { get; set; }

        public IList<Tuple<ParenthesisExpression, CodeBlock>> ElseIfs { get; set; }

        private ParenthesisExpression _cache;

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            WrapIfStatement(ref symbol);

            if (Is<ParenthesisExpression>(symbol))
            {
                if (Condition == null)
                {
                    symbol.Role = "Condition";
                    Condition = (ParenthesisExpression)symbol;
                    return;
                }
                _cache = (ParenthesisExpression)symbol;
                return;
            }

            if (Is<CodeBlock>(symbol))
            {
                if (TrueCodeBlock==null)
                {
                    symbol.Role = "TrueCodeBlock";
                    TrueCodeBlock = (CodeBlock) symbol;
                    return;
                }

                if (ElseCodeBlock==null && _cache == null)
                {
                    symbol.Role = "ElseCodeBlock";
                    ElseCodeBlock = (CodeBlock) symbol;
                    return;
                }
                _cache.Role = "ElseIfCondition";
                symbol.Role = "ElseIfCodeBlock";
                ElseIfs.Add(new Tuple<ParenthesisExpression, CodeBlock>(_cache,(CodeBlock) symbol));
                _cache = null;
                return;
            }
            
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (Condition != null) yield return Condition;
            if (ElseIfs!=null)
            {
                foreach (var elseIf in ElseIfs)
                {
                    yield return elseIf.Item1;
                    yield return elseIf.Item2;
                }
            }
            if (ElseCodeBlock != null) yield return ElseCodeBlock;
        }

        public StatementLabel Label { get; set; }
    }
}