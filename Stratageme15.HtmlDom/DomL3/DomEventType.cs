

namespace Stratageme15.HtmlDom.DomL3
{
    public  class DomEventType : BaseEventType
    {
        internal DomEventType(string type) : base(type)
        {
        }

        /// <summary>
        /// The DOMFocusIn event occurs when an EventTarget receives focus, for instance via a pointing device being moved 
        /// onto an element or by tabbing navigation to the element. 
        /// Unlike the HTML event focus, DOMFocusIn can be applied to any focusable EventTarget, not just FORM controls
        /// </summary>
        public static DomEventType DOMFocusIn { get { return Evt<DomEventType>("DOMFocusIn"); } }

        /// <summary>
        /// The DOMFocusOut event occurs when a EventTarget loses focus, for instance via a pointing device 
        /// being moved out of an element or by tabbing navigation out of the element. 
        /// Unlike the HTML event blur, DOMFocusOut can be applied to any focusable EventTarget, not just FORM controls
        /// </summary>
        public static DomEventType DOMFocusOut { get { return Evt<DomEventType>("DOMFocusOut"); } }

        /// <summary>
        /// The activate event occurs when an element is activated, for instance, thru a mouse click or a keypress. 
        /// A numerical argument is provided to give an indication of the type of activation that occurs: 1 for a simple activation (e.g. a simple click or Enter), 2 for hyperactivation (for instance a double click or Shift Enter)
        /// </summary>
        public static DomEventType DOMActivate { get { return Evt<DomEventType>("DOMActivate"); } }
    }
}
