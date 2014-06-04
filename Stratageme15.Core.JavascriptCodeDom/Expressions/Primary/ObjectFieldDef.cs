using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Primary
{
    public class ObjectFieldDef : SyntaxTreeNodeBase
    {
        public IDictionaryKey Key { get; set; }

        public Expression Value { get; set; }
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Is<IDictionaryKey>(symbol))
            {
                if (Key==null)
                {
                    Key = (IDictionaryKey) symbol;
                    return;
                }
            }
            if (Is<Expression>(symbol))
            {
                if (Value == null)
                {
                    Value = (Expression)symbol;
                    return;
                }
            }
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (Key != null) yield return (SyntaxTreeNodeBase) Key;
            if (Value != null) yield return Value;
        }
    }
}
