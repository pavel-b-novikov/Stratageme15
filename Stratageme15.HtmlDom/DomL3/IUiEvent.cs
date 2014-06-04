


namespace Stratageme15.HtmlDom.DomL3
{
    /// <summary>
    /// The UIEvent interface provides specific contextual information associated with User Interface events
    /// </summary>
    public interface IUiEvent : IEvent
    {
        /// <summary>
        /// The name of the event (case-insensitive). The name must be an XML name
        /// </summary>
        new DomEventType Type { get; }

        /// <summary>
        /// Specifies some detail information about the Event, depending on the type of event
        /// </summary>
        long Detail { get; }

        /// <summary>
        /// The view attribute identifies the AbstractView from which the event was generated
        /// </summary>
        dynamic View { get; }

        /// <summary>
        /// The initUIEvent method is used to initialize the value of a UIEvent created through the DocumentEvent interface. 
        /// This method may only be called before the UIEvent has been dispatched via the dispatchEvent method,
        /// though it may be called multiple times during that phase if necessary. 
        /// If called multiple times, the final invocation takes precedence
        /// </summary>
        /// <param name="type">Specifies the event type</param>
        /// <param name="canBubble">Specifies whether or not the event can bubble</param>
        /// <param name="cancelable">Specifies whether or not the event's default action can be prevented</param>
        /// <param name="view">Specifies the Event's AbstractView</param>
        /// <param name="detail">Specifies the Event's detail</param>
        void InitUIEvent(BaseEventType type, bool canBubble, bool cancelable, object view, long detail);
    }
}
