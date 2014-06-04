namespace Stratageme15.HtmlDom.Html5
{
    public interface IInputEventTarget
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
        void AddEventListener(InputEventType type, EventDelegate<IEvent> listener, bool useCapture = false);

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
        void AddEventListener<TThis>(InputEventType type, ContextEventDelegate<TThis, IEvent> listener, bool useCapture = false);
    }
}
