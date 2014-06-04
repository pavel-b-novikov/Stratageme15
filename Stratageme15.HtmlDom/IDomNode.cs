namespace Stratageme15.HtmlDom
{
    /// <summary>
    /// In the HTML DOM, the Element object represents an HTML element.
    /// Element objects can have child nodes of type element nodes, text nodes, or comment nodes.
    /// A NodeList object represents a list of nodes, like an HTML element's collection of child nodes.
    /// Elements can also have attributes. Attributes are attribute nodes (See next chapter).
    /// </summary>
    public interface IDomNode
    {
       
        /// <summary>
        /// Appends a node as the last child of a node.
        /// </summary>
        /// <param name="node">Required. The node object you want to append</param>
        /// <returns>The appended node</returns>
        IDomNode AppendChild(IDomNode node);

        /// <summary>
        /// Gets a NamedNodeMap object, representing a collection of attributes
        /// </summary>
        INamedNodeMap<IDomNode> Attributes { get; }

        /// <summary>
        /// Returns the absolute base URL of a node
        /// </summary>
        string BaseURI { get; }

        /// <summary>
        /// Gets a NamedNodeMap object, representing a collection of attributes
        /// </summary>
        INodeList<IDomNode> ChildNodes { get; }

        /// <summary>
        /// Returns the node's first child in the tree, or null if the node is childless. If the node is a Document, it returns the first node in the list of its direct children.
        /// </summary>
        IDomNode FirstChild { get; }

        /// <summary>
        /// Returns the last child of a node
        /// </summary>
        IDomNode LastChild { get; }

        /// <summary>
        /// Returns the local part of the qualified name of this node
        /// </summary>
        string LocalName { get; }

        /// <summary>
        /// The namespace URI of the node, or null if the node is not in a namespace (read-only). When the node is a document, it returns the XML namespace for the current document
        /// </summary>
        string NamespaceURI { get; }

        /// <summary>
        /// Returns the node immediately following the specified one in its parent's childNodes list, or null if the specified node is the last node in that list
        /// </summary>
        IDomNode NextSibling { get; }

        /// <summary>
        /// Returns the name of the current node as a string
        /// </summary>
        string NodeName { get; }

        /// <summary>
        /// The read-only Node.nodeType property returns an unsigned short integer representing the type of the node
        /// </summary>
        NodeTypeEnum NodeType { get; }

        /// <summary>
        /// Returns or sets the value of the current node
        /// </summary>
        string NodeValue { get; set; }

        /// <summary>
        /// The ownerDocument property returns the top-level document object for this node
        /// </summary>
        IDocument OwnerDocument { get; }

        /// <summary>
        /// Returns the DOM node's parent Element, or null if the node either has no parent, or its parent isn't a DOM Element
        /// </summary>
        IElement ParentElement { get; }

        /// <summary>
        /// Returns the parent of the specified node in the DOM tree
        /// </summary>
        IDomNode ParentNode { get; }

        /// <summary>
        /// Returns the namespace prefix of the specified node, or null if no prefix is specified. This property is read only
        /// </summary>
        string Prefix { get; }

        /// <summary>
        /// Returns the node immediately preceding the specified one in its parent's childNodes list, null if the specified node is the first in that list
        /// </summary>
        IDomNode PreviousSibling { get; }

        /// <summary>
        /// Gets or sets the text content of a node and its descendents
        /// </summary>
        string TextContent { get; set; }
    }
}
