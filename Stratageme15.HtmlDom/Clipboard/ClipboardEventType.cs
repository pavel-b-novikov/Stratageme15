

namespace Stratageme15.HtmlDom.Clipboard
{
    public class ClipboardEventType : BaseEventType
    {
        internal ClipboardEventType(string type) : base(type)
        {
        }

        /// <summary>
        /// The copy event is fired when a selection has been added to the clipboard
        /// </summary>
        public static ClipboardEventType Copy { get { return Evt<ClipboardEventType>("copy"); } }

        /// <summary>
        /// The cut event is fired when a selection has been removed from the document and added to the clipboard
        /// </summary>
        public static ClipboardEventType Cut { get { return Evt<ClipboardEventType>("cut"); } }

        /// <summary>
        /// The paste event is fired when a selection has been pasted from the clipboard to the document
        /// </summary>
        public static ClipboardEventType Paste { get { return Evt<ClipboardEventType>("paste"); } }

    }
}
