using Microsoft.CodeAnalysis;
using Stratageme15.Core.Translation.Reactors;

namespace Stratageme15.Core.Translation
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