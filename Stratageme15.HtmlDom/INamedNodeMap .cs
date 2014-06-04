using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratageme15.HtmlDom
{
    /// <summary>
    /// The nodes in the NamedNodeMap can be accessed through their name.
    /// The NamedNodeMap keeps itself up-to-date. If an element is deleted or added, in the node list or the XML document, the list is automatically updated.
    /// Note: In a named node map, the nodes are not returned in any particular order.
    /// </summary>
    public interface INamedNodeMap<TNode> : INodeList<TNode>
        where TNode : IDomNode
    {
      
        /// <summary>
        /// Returns the node with the specific name
        /// </summary>
        /// <param name="nodeName">The name of the node to return</param>
        /// <returns>The node with the specified name, or null if it does not identify any node in the map</returns>
        TNode GetNamedItem(string nodeName);

        /// <summary>
        /// Retrieves a node specified by local name and namespace URI.
        /// </summary>
        /// <param name="namespaceURI">The namespace URI of the node to retrieve</param>
        /// <param name="localName">The local name of the node to retrieve.</param>
        /// <returns>A Node (of any type) with the specified local name and namespace URI, or null if they do not identify any node in this map.</returns>
        TNode GetNamedItemNs(string namespaceURI, string localName);

        /// <summary>
        /// Removes the node with the specific name
        /// </summary>
        /// <param name="nodeName">The name of the node to remove</param>
        void RemoveNamedItem(string nodeName);

        /// <summary>
        /// Removes a node specified by local name and namespace URI.
        /// </summary>
        /// <param name="namespaceURI">The namespace URI of the node to remove</param>
        /// <param name="localName">The local name of the node to remove</param>
        void RemoveNamedItemNs(string namespaceURI, string localName);

        /// <summary>
        /// Adds a node using its nodeName attribute
        /// </summary>
        /// <param name="node">A node to store in this map</param>
        /// <returns>If the new Node replaces an existing node the replaced Node is returned, otherwise null is returned</returns>
        TNode SetNamedItem(TNode node);

        /// <summary>
        /// Adds a node using its namespaceURI and localName.
        /// </summary>
        /// <param name="node">A node to store in this map</param>
        /// <returns>If the new Node replaces an existing node the replaced Node is returned, otherwise null is returned</returns>
        TNode SetNamedItemNs(TNode node);
    }
}
