

namespace Stratageme15.HtmlDom.DomL3
{
    public class ObjectEventType : BaseEventType
    {
        internal ObjectEventType(string type) : base(type)
        {
        }

        /// <summary>
        /// The load event occurs when the DOM implementation finishes loading all content within a document, all frames within a FRAMESET, or an OBJECT element.
        /// </summary>
        public static ObjectEventType Load { get { return Evt<ObjectEventType>("load"); } }

        /// <summary>
        /// The abort event occurs when page loading is stopped before an image has been allowed to completely load. This event applies to OBJECT elements.
        /// </summary>
        public static ObjectEventType Abort { get { return Evt<ObjectEventType>("abort"); } }

        /// <summary>
        /// The error event occurs when an image does not load properly or when an error occurs during script execution. This event is valid for OBJECT elements, BODY elements, and FRAMESET element.
        /// </summary>
        public static ObjectEventType Error { get { return Evt<ObjectEventType>("error"); } }
    }

}
