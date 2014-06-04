

namespace Stratageme15.HtmlDom.Html5
{
    public class FormEventType : BaseEventType
    {
        internal FormEventType(string type) : base(type)
        {
        }

        /// <summary>
        /// The submit event occurs when a form is submitted. This event only applies to the FORM element.
        /// </summary>
        public static FormEventType Submit { get { return Evt<FormEventType>("submit"); } }

        /// <summary>
        /// The reset event occurs when a form is reset. This event only applies to the FORM element.
        /// </summary>
        public static FormEventType Reset { get { return Evt<FormEventType>("reset"); } }
    }
}
