using Roslyn.Compilers.CSharp;
using Stratageme15.Core.Transaltion.Reactors;

namespace Stratageme15.Core.Transaltion
{
    internal class ChildrenTraversalPromise
    {
        public ChildrenTraversalPromise(int stackPosition, IReactor promisee, SyntaxNode originalNode)
        {
            OriginalNode = originalNode;
            StackPosition = stackPosition;
            Promisee = promisee;
        }

        public int StackPosition { get; private set; }
        public IReactor Promisee { get; private set; }
        public SyntaxNode OriginalNode { get; private set; }

    }
}
