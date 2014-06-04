namespace Stratageme15.HtmlDom
{
    /// <summary>
    /// The Element interface represents an object within a DOM document. 
    /// This interface describes methods and properties common to all kinds of elements. 
    /// Specific behaviors are described in interfaces which inherit from 
    /// Element but add additional functionality. For example, 
    /// the HTMLElement interface is the base interface for HTML elements, 
    /// while the SVGElement interface is the basis for all SVG elements
    /// </summary>
    public interface IElement : IDomNode
    {
        /// <summary>
        /// Sets or returns the accesskey for an element
        /// </summary>
        string AccessKey { get; set; }

    }
}
