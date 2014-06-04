

namespace Stratageme15.HtmlDom.DomL3
{
    public enum AttrChangeType
    {
        /// <summary>
        /// The Attr was modified in place
        /// </summary>
        MODIFICATION = 1,
        /// <summary>
        /// The Attr was just added.
        /// </summary>
        ADDITION = 2,
        /// <summary>
        /// The Attr was just removed
        /// </summary>
        REMOVAL = 3
    }
    /// <summary>
    /// The MutationEvent interface provides specific contextual information associated with Mutation events
    /// </summary>
    public interface IMutationEvent : IEvent
    {
        /// <summary>
        /// The name of the event (case-insensitive). The name must be an XML name
        /// </summary>
        new MutationEventType Type { get; }

        /// <summary>
        ///  indicates the type of change which triggered the DOMAttrModified event. The values can be MODIFICATION, ADDITION, or REMOVAL
        /// </summary>
        AttrChangeType AttrChange { get; }

        /// <summary>
        /// Indicates the name of the changed Attr node in a DOMAttrModified event
        /// </summary>
        string AttrName { get; }

        /// <summary>
        /// indicates the new value of the Attr node in DOMAttrModified events, and of the CharacterData node in DOMCharDataModified events
        /// </summary>
        string NewValue { get; }

        /// <summary>
        ///  indicates the previous value of the Attr node in DOMAttrModified events, and of the CharacterData node in DOMCharDataModified events
        /// </summary>
        string PrevValue { get; }

        /// <summary>
        /// is used to identify a secondary node related to a mutation event. 
        /// For example, if a mutation event is dispatched to a node indicating that 
        /// its parent has changed, the relatedNode is the changed parent. 
        /// If an event is instead dispatched to a subtree indicating a node was changed within it, 
        /// the relatedNode is the changed node. 
        /// In the case of the DOMAttrModified event it indicates the Attr node which was modified, added, or removed
        /// </summary>
        IDomNode RelatedNode { get; }
    }
}
