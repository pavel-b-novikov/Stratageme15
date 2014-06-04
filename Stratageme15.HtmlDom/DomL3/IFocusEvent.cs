

namespace Stratageme15.HtmlDom.DomL3
{
    /// <summary>
    /// The FocusEvent interface represents focus-related events like focus, blur, focusin, or focusout
    /// </summary>
    public interface IFocusEvent : IUiEvent
    {
        /// <summary>
        /// Is an EventTarget representing a secondary target for this event. As in some cases (like when tabbing in or out a page), this property may be set to null for security reasons.
        /// </summary>
        IEventTarget RelatedTarget { get; }
    }
}
