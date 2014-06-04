

namespace Stratageme15.HtmlDom.DomL3
{
    public class FramesetEventType : BaseEventType
    {
        internal FramesetEventType(string type) : base(type)
        {
        }
        /// <summary>
        /// The load event occurs when the DOM implementation finishes loading all content within a document, all frames within a FRAMESET, or an OBJECT element.
        /// </summary>
        public static FramesetEventType Load { get { return Evt<FramesetEventType>("load"); } }

        /// <summary>
        /// The unload event occurs when the DOM implementation removes a document from a window or frame. This event is valid for BODY and FRAMESET elements.
        /// </summary>
        public static FramesetEventType Unload { get { return Evt<FramesetEventType>("unload"); } }

        /// <summary>
        /// The error event occurs when an image does not load properly or when an error occurs during script execution. This event is valid for OBJECT elements, BODY elements, and FRAMESET element.
        /// </summary>
        public static FramesetEventType Error { get { return Evt<FramesetEventType>("error"); } }
    }
}
