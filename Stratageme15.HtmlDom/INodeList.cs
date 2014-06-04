using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratageme15.HtmlDom
{
    /// <summary>
    /// NodeList objects are collections of nodes such as those returned by Node.childNodes and the querySelectorAll method
    /// </summary>
    /// <typeparam name="TNode">type of child Nodes</typeparam>
    public interface INodeList <TNode> where TNode : IDomNode
    {
        /// <summary>
        /// Returns the number of nodes in the list
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Returns the node at the specified index
        /// (migrated from item() method)
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>A node with specified index</returns>
        TNode this[int index] { get; }

    }
}
