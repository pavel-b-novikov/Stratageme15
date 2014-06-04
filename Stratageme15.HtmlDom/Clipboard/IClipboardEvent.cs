using Stratageme15.HtmlDom.DomL3;



namespace Stratageme15.HtmlDom.Clipboard
{
    /// <summary>
    /// The ClipboardEvent interface represents events providing information related to modification of the clipboard, that is cut, copy, and paste events.
    /// </summary>
    public interface IClipboardEvent : IEvent
    {
        /// <summary>
        /// Is a DataTransfer object containing the data affected by the user-initialed cut, copy, or paste operation, along with its MIME type.
        /// </summary>
        IDataTransfer ClipboardData { get; }

        /// <summary>
        /// The name of the event (case-insensitive). The name must be an XML name
        /// </summary>
        new ClipboardEventType Type { get; }
    }
}
