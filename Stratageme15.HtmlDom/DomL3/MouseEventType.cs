

namespace Stratageme15.HtmlDom.DomL3
{
    public class MouseEventType : BaseEventType
    {
        internal MouseEventType(string type) : base(type)
        {
        }

        /// <summary>
        /// The click event occurs when the pointing device button is clicked over an element. 
        /// A click is defined as a mousedown and mouseup over the same screen location. The sequence of these events is:
        /// mousedown
        /// mouseup
        /// click
        /// If multiple clicks occur at the same screen location, the sequence repeats with the detail attribute incrementing with each repetition. This event is valid for most elements
        /// </summary>
        public static MouseEventType Click { get { return Evt<MouseEventType>("click"); } }

        /// <summary>
        /// The mousedown event occurs when the pointing device button is pressed over an element. This event is valid for most elements
        /// </summary>
        public static MouseEventType MouseDown { get { return Evt<MouseEventType>("mousedown"); } }

        /// <summary>
        /// The mouseup event occurs when the pointing device button is released over an element. This event is valid for most elements
        /// </summary>
        public static MouseEventType MouseUp { get { return Evt<MouseEventType>("mouseup"); } }

        /// <summary>
        /// The mouseover event occurs when the pointing device is moved onto an element. This event is valid for most elements
        /// </summary>
        public static MouseEventType MouseOver { get { return Evt<MouseEventType>("mouseover"); } }

        /// <summary>
        /// The mousemove event occurs when the pointing device is moved while it is over an element. This event is valid for most elements.
        /// </summary>
        public static MouseEventType MouseMove { get { return Evt<MouseEventType>("mousemove"); } }

        /// <summary>
        /// The mouseout event occurs when the pointing device is moved away from an element. This event is valid for most elements.
        /// </summary>
        public static MouseEventType MouseOut { get { return Evt<MouseEventType>("mouseout"); } }
    }
}
