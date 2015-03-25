using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Extensions;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class CatchClause : SyntaxTreeNodeBase
    {
        public IdentExpression Identifier { get; set; }

        public CodeBlock Handler { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Is<IdentExpression>(symbol))
            {
                if (Identifier == null)
                {
                    Identifier = (IdentExpression)symbol;
                    symbol.Parent = this;
                    return;
                }
            }

            if (Is<IStatement>(symbol))
            {
                symbol = symbol.WrapInCodeBlock();
            }

            

            if (Is<CodeBlock>(symbol))
            {
                if (Handler==null)
                {
                    Handler = (CodeBlock)symbol;
                    symbol.Parent = this;
                    return;
                }
            }
            
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (Identifier != null) yield return Identifier;
            if (Handler != null) yield return Handler;
        }
    }
}
