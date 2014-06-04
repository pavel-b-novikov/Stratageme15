

namespace Stratageme15.HtmlDom.DomL3
{
    public class FocusEventType : BaseEventType
    {
        internal FocusEventType(string type) : base(type)
        {
        }

        /// <summary>
        /// The focus event is fired when an element has received focus. The main difference between this event and focusin is that only the latter bubbles.
        /// </summary>
        public static FocusEventType Focus { get { return Evt<FocusEventType>("focus"); } }

        /// <summary>
        /// The blur event is fired when an element has lost focus. The main difference between this event and focusout is that only the latter bubbles
        /// </summary>
        public static FocusEventType Blur { get { return Evt<FocusEventType>("blur"); } }

        /// <summary>
        /// The focusin event is fired when an element is about to receive focus. The main difference between this event and focus is that the latter doesn't bubble
        /// </summary>
        public static FocusEventType FocusIn { get { return Evt<FocusEventType>("focusin"); } }

        /// <summary>
        /// The focusout event is fired when an element is about to lose focus. The main difference between this event and blur is that the latter doesn't bubble
        /// </summary>
        public static FocusEventType FocusOut { get { return Evt<FocusEventType>("focusout"); } }
    }
}
