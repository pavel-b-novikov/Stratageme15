using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Exceptions;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom
{
    public class JsProgram : SyntaxTreeNodeBase
    {
        public JsProgram()
        {
            CodeElements = new List<IRootCodeElement>();
        }

        public IList<IRootCodeElement> CodeElements { get; private set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Is<IRootCodeElement>(symbol))
            {
                CodeElements.Add((IRootCodeElement) symbol);
                return;
            }

            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            foreach (var rootCodeElement in CodeElements)
            {
                yield return (SyntaxTreeNodeBase) rootCodeElement;
            }
        }
    }
}