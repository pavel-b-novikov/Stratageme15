using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratageme15.HtmlDom
{
    /// <summary>
    /// The File interface provides information about -- and access to the contents of -- files
    /// These are generally retrieved from a FileList object returned as a result of a user selecting files 
    /// using the <input> element, from a drag and drop operation's DataTransfer object, or from the mozGetAsFile() API on an HTMLCanvasElement.
    /// </summary>
    public interface IInputFile
    {
        /// <summary>
        /// The name of the file referenced by the File object.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Returns the last modified date of the file. Files without a known last modified date use the current date instead.
        /// </summary>
        dynamic LastModifiedDate { get; }

        /// <summary>
        /// Returns the name of the file. For security reasons, the path is excluded from this property.
        /// </summary>
        string Name { get; }
    }
}
