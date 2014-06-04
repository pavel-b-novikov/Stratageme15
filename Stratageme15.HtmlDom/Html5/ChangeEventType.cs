

namespace Stratageme15.HtmlDom.Html5
{
    public class ChangeEventType : BaseEventType
    {
        internal ChangeEventType(string type) : base(type)
        {
        }

        /// <summary>
        /// The change event occurs when a control loses the input focus and its value has been modified since gaining focus. 
        /// This event is valid for INPUT, SELECT, and TEXTAREA. element.
        /// </summary>
        public static ChangeEventType Change { get { return Evt<ChangeEventType>("change"); } }
    }
}
