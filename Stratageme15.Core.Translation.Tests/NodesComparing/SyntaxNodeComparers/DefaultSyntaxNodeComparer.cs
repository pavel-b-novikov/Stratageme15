using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Reactors.Basic.Tests.NodesComparing.SyntaxNodeComparers
{
    public class DefaultSyntaxNodeComparer : ISyntaxNodeComparer
    {
        public bool EqualsExceptChildren(SyntaxTreeNodeBase expected, SyntaxTreeNodeBase actual)
        {
            if (expected == null && actual == null) return true;
            if (expected == null || actual == null) return false;
            if (expected is IStatement)
            {
                IStatement e = expected as IStatement;
                IStatement a = actual as IStatement;
                if (!CompareNodes(e.Label, a.Label)) return false;
            }
            return true;
        }

        protected bool CompareNodes(SyntaxTreeNodeBase expected, SyntaxTreeNodeBase actual)
        {
            if (expected == null && actual == null) return true;
            if (expected == null || actual == null) return false;
            if (expected.GetType() != actual.GetType()) return false;
            var comparer = NodeComparerFactory.GetComparerFor(expected.GetType());
            return comparer.EqualsExceptChildren(expected, actual);
        }
    }
}
