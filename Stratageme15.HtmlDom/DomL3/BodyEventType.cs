

namespace Stratageme15.HtmlDom.DomL3
{
    public class BodyEventType : BaseEventType
    {
        internal BodyEventType(string type) : base(type)
        {
        }

        /// <summary>
        /// The unload event occurs when the DOM implementation removes a document from a window or frame. This event is valid for BODY and FRAMESET elements.
        /// </summary>
        public static BodyEventType Unload { get { return Evt<BodyEventType>("unload"); } }

        /// <summary>
        /// The error event occurs when an image does not load properly or when an error occurs during script execution. This event is valid for OBJECT elements, BODY elements, and FRAMESET element.
        /// </summary>
        public static BodyEventType Error { get { return Evt<BodyEventType>("error"); } }
    }
}
