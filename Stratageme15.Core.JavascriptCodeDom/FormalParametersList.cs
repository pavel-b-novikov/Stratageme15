﻿using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.JavascriptCodeDom
{
    public class FormalParametersList : SyntaxTreeNodeBase
    {
        public IList<IdentExpression> ArgumentNames { get; private set; }

        public FormalParametersList(IList<IdentExpression> argumentNames)
        {
            ArgumentNames = argumentNames;
        }

        public FormalParametersList()
        {
            ArgumentNames = new List<IdentExpression>();
        }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (symbol is IdentExpression)
            {
                ArgumentNames.Add((IdentExpression) symbol);
                return;
            }
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            return ArgumentNames;
        }
    }
}