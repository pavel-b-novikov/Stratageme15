namespace Stratageme15.HtmlDom
{
    /// <summary>
    /// EventTarget is a DOM interface implemented by objects that can receive DOM events and have listeners for them
    /// </summary>
    public interface IEventTarget
    {
        /// <summary>
        /// Register an event handler of a specific event type on the EventTarget
        /// </summary>
        /// <param name="type">A event type enum representing the event type to listen for</param>
        /// <param name="listener">The object that receives a notification when an event of the specified type occurs. This must be an object implementing the EventListener interface, or simply a JavaScript function</param>
        /// <param name="useCapture">
        /// If true, useCapture indicates that the user wishes to initiate capture. 
        /// After initiating capture, all events of the specified type will be dispatched to the registered listener before being dispatched to any EventTarget beneath it in the DOM tree. 
        /// Events which are bubbling upward through the tree will not trigger a listener designated to use capture. See DOM Level 3 Events for a detailed explanation. 
        /// If not specified, useCapture defaults to false.
        /// </param>
        void AddEventListener<TEvent>(string type, EventDelegate<TEvent> listener, bool useCapture = false) where TEvent : IEvent;

        /// <summary>
        /// Dispatches an Event at the specified EventTarget, invoking the affected EventListeners in the appropriate order. 
        /// The normal event processing rules (including the capturing and optional bubbling phase) apply to events dispatched manually with dispatchEvent()
        /// </summary>
        /// <param name="evt">The Event object to be dispatched</param>
        /// <returns>The return value is false if at least one of the event handlers which handled this event called Event.preventDefault(). Otherwise it returns true</returns>
        bool DispatchEvent(IEvent evt);

        /// <summary>
        /// Removes the event listener previously registered with EventTarget.addEventListener
        /// </summary>
        /// <param name="type">A event type enum representing the event type being removed</param>
        void RemoveEventListener(string type);
    }
}
