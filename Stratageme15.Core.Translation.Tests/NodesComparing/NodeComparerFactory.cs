using System;
using System.Collections.Generic;
using System.Linq;
using Stratageme15.Reactors.Basic.Tests.NodesComparing.SyntaxNodeComparers;

namespace Stratageme15.Reactors.Basic.Tests.NodesComparing
{
    public static class NodeComparerFactory
    {
        static NodeComparerFactory()
        {
            var a = typeof(ISyntaxNodeComparer).Assembly;
            var types = a.GetTypes().Where(t => typeof(ISyntaxNodeComparer).IsAssignableFrom(t) && !t.IsAbstract &&!t.IsAbstract);
            foreach (var type in types)
            {
                var ga = type.BaseType.GetGenericArguments();
                if (ga.Length==0) continue;
                var key = ga[0];
                var value = Activator.CreateInstance(type);
                _nodeComparers.Add(key, (ISyntaxNodeComparer)value);
            }
        }

        private static readonly DefaultSyntaxNodeComparer _default = new DefaultSyntaxNodeComparer();

        private static readonly Dictionary<Type, ISyntaxNodeComparer> _nodeComparers = new Dictionary<Type, ISyntaxNodeComparer>();

        public static ISyntaxNodeComparer GetComparerFor(Type nodeType)
        {
            if (_nodeComparers.ContainsKey(nodeType)) return _nodeComparers[nodeType];
            var bt = nodeType.BaseType;
            while (bt != null)
            {
                if (_nodeComparers.ContainsKey(bt)) return _nodeComparers[bt];
                bt = bt.BaseType;
            }
            return _default;
        }
    }
}
