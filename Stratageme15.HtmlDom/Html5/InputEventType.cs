namespace Stratageme15.HtmlDom.Html5
{
    public class InputEventType : BaseEventType
    {
        internal InputEventType(string type) : base(type)
        {
        }

        /// <summary>
        /// The select event occurs when a user selects some text in a text field. This event is valid for INPUT and TEXTAREA elements.
        /// </summary>
        public static InputEventType Select { get { return Evt<InputEventType>("select"); } }

        /// <summary>
        /// The change event occurs when a control loses the input focus and its value has been modified since gaining focus. 
        /// This event is valid for INPUT, SELECT, and TEXTAREA. element.
        /// </summary>
        public static InputEventType Change { get { return Evt<InputEventType>("change"); } }
    }
}
