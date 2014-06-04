

namespace Stratageme15.HtmlDom.DomL3
{
    /// <summary>
    /// The MouseEvent interface provides specific contextual information associated with Mouse events.
    /// The detail attribute inherited from UIEvent indicates the number of times a mouse button 
    /// has been pressed and released over the same screen location during a user action. 
    /// The attribute value is 1 when the user begins this action and increments by 1 for each full 
    /// sequence of pressing and releasing. 
    /// If the user moves the mouse between the mousedown and mouseup the value will be set to 0, 
    /// indicating that no click is occurring.
    /// In the case of nested elements mouse events are always targeted at the most deeply nested element. 
    /// Ancestors of the targeted element may use bubbling to obtain notification 
    /// of mouse events which occur within its descendent elements.
    /// </summary>
    public interface IMouseEvent : IUiEvent
    {
        /// <summary>
        /// The name of the event (case-insensitive). The name must be an XML name
        /// </summary>
        new MouseEventType Type { get; }

        /// <summary>
        /// The horizontal coordinate at which the event occurred relative to the origin of the screen coordinate system.
        /// </summary>
        long ScreenX { get; }

        /// <summary>
        /// The vertical coordinate at which the event occurred relative to the origin of the screen coordinate system.
        /// </summary>
        long ScreenY { get; }

        /// <summary>
        /// The horizontal coordinate at which the event occurred relative to the DOM implementation's client area.
        /// </summary>
        long ClientX { get; }

        /// <summary>
        /// The vertical coordinate at which the event occurred relative to the DOM implementation's client area.
        /// </summary>
        long ClientY { get; }

        /// <summary>
        /// Used to indicate whether the 'ctrl' key was depressed during the firing of the event.
        /// </summary>
        bool CtrlKey { get; }

        /// <summary>
        /// Used to indicate whether the 'shift' key was depressed during the firing of the event.
        /// </summary>
        bool ShiftKey { get; }

        /// <summary>
        /// Used to indicate whether the 'alt' key was depressed during the firing of the event. On some platforms this key may map to an alternative key name.
        /// </summary>
        bool AltKey { get; }

        /// <summary>
        /// Used to indicate whether the 'meta' key was depressed during the firing of the event. On some platforms this key may map to an alternative key name.
        /// </summary>
        bool MetaKey { get; }

        /// <summary>
        /// During mouse events caused by the depression or release of a mouse button, 
        /// button is used to indicate which mouse button changed state. 
        /// The values for button range from zero to indicate the left button of the mouse, 
        /// one to indicate the middle button if present, and two to indicate the right button. 
        /// For mice configured for left handed use in which the button actions 
        /// are reversed the values are instead read from right to left.
        /// </summary>
        short Button { get; }

        /// <summary>
        /// Used to identify a secondary EventTarget related to a UI event. 
        /// Currently this attribute is used with the mouseover event to indicate the EventTarget 
        /// which the pointing device exited and with the mouseout event to indicate the EventTarget which the pointing device entered.
        /// </summary>
        IEventTarget RelatedTarget { get; }

        /// <summary>
        /// The initMouseEvent method is used to initialize the value of a MouseEvent created through the DocumentEvent interface. 
        /// This method may only be called before the MouseEvent has been dispatched via the dispatchEvent method, 
        /// though it may be called multiple times during that phase if necessary. 
        /// If called multiple times, the final invocation takes precedence.
        /// </summary>
        /// <param name="eventType">Specifies the event type.</param>
        /// <param name="canBubble">Specifies whether or not the event can bubble.</param>
        /// <param name="cancelable">Specifies whether or not the event's default action can be prevented.</param>
        /// <param name="view">Specifies the Event's AbstractView.</param>
        /// <param name="detail">Specifies the Event's mouse click count</param>
        /// <param name="screenX">Specifies the Event's screen x coordinate</param>
        /// <param name="screenY">Specifies the Event's screen y coordinate</param>
        /// <param name="clientX">Specifies the Event's client x coordinate</param>
        /// <param name="clientY">Specifies the Event's client y coordinate</param>
        /// <param name="ctrlKey">Specifies whether or not control key was depressed during the Event.</param>
        /// <param name="altKey">Specifies whether or not alt key was depressed during the Event.</param>
        /// <param name="shiftKey">Specifies whether or not shift key was depressed during the Event.</param>
        /// <param name="metaKey">Specifies whether or not meta key was depressed during the Event.</param>
        /// <param name="button">Specifies the Event's mouse button.</param>
        /// <param name="relatedTarget">Specifies the Event's related EventTarget.</param>
        void InitMouseEvent(MouseEventType eventType, bool canBubble, bool cancelable, object view, long detail,
                            long screenX, long screenY, long clientX, long clientY, bool ctrlKey, bool altKey,
                            bool shiftKey, bool metaKey, short button, IEventTarget relatedTarget);
    }
}
