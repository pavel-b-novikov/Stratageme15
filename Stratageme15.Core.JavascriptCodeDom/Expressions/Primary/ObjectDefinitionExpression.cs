using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Primary
{
    public class ObjectDefinitionExpression : PrimaryExpression
    {
        public ObjectDefinitionExpression()
        {
            ObjectFields = new List<ObjectFieldDef>();
            isCollectingKey = true;
        }

        public List<ObjectFieldDef> ObjectFields { get; private set; }

        private IDictionaryKey _cache;

        private bool isCollectingKey;

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Is<ObjectFieldDef>(symbol))
            {
                ObjectFields.Add((ObjectFieldDef) symbol);
                return;
            }

            if (isCollectingKey)
            {
                if (Is<IDictionaryKey>(symbol))
                {
                    symbol.Role = "Key";
                    _cache = (IDictionaryKey) symbol;
                    isCollectingKey = false;
                    return;
                }
            }else
            {
                if (Is<Expression>(symbol))
                {
                    symbol.Role = "Value";
                    ObjectFieldDef ofd = new ObjectFieldDef();
                    ofd.Key = _cache;
                    ofd.Value = (Expression) symbol;
                    ofd.Parent = this;
                    ObjectFields.Add(ofd);
                    isCollectingKey = true;
                    return;
                }
            }
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            return ObjectFields;
        }
    }
}