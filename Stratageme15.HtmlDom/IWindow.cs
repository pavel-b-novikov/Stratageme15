using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratageme15.HtmlDom
{
    /// <summary>
    /// The window object represents an open window in a browser.
    /// If a document contain frames (<frame> or <iframe> tags), 
    /// the browser creates one window object for the HTML document, 
    /// and one additional window object for each frame.
    /// </summary>
    public interface IWindow
    {
        /// <summary>
        /// Returns a Boolean value indicating whether a window has been closed or not
        /// </summary>
        bool Closed { get; }

       

    }
}
