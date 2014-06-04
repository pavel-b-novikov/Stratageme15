

namespace Stratageme15.HtmlDom.DomL3
{
    public  class DocumentEventType : BaseEventType
    {
        internal DocumentEventType(string type) : base(type)
        {
        }

        /// <summary>
        /// The resize event occurs when a document view is resized.
        /// </summary>
        public static DocumentEventType Resize { get { return Evt<DocumentEventType>("resize"); } }

        /// <summary>
        /// The scroll event occurs when a document view is scrolled.
        /// </summary>
        public static DocumentEventType Scroll { get { return Evt<DocumentEventType>("scroll"); } }
    }
}
