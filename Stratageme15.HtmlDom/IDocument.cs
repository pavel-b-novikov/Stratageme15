using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratageme15.HtmlDom
{
    /// <summary>
    /// When an HTML document is loaded into a web browser, it becomes a document object.
    /// The document object is the root node of the HTML document and the "owner" of all other nodes:
    /// (element nodes, text nodes, attribute nodes, and comment nodes).
    /// The document object provides properties and methods to access all node objects, from within JavaScript.
    /// </summary>
    public interface IDocument : IDomNode
    {
    }
}
