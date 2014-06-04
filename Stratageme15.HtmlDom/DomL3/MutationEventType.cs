

namespace Stratageme15.HtmlDom.DomL3
{
    public class MutationEventType: BaseEventType
    {
        internal MutationEventType(string type) : base(type)
        {
        }

        /// <summary>
        /// This is a general event for notification of all changes to the document. 
        /// It can be used instead of the more specific events listed below. 
        /// It may be fired after a single modification to the document or, at the implementation's discretion, after multiple changes have occurred. 
        /// The latter use should generally be used to accomodate multiple changes which occur either simultaneously or in rapid succession. 
        /// The target of this event is the lowest common parent of the changes which have taken place. 
        /// This event is dispatched after any other events caused by the mutation have fired.
        /// </summary>
        public static MutationEventType DOMSubtreeModified { get { return Evt<MutationEventType>("DOMSubtreeModified"); } }

        /// <summary>
        /// Fired when a node has been added as a child of another node. This event is dispatched after the insertion has taken place. The target of this event is the node being inserted.
        /// </summary>
        public static MutationEventType DOMNodeInserted { get { return Evt<MutationEventType>("DOMNodeInserted"); } }

        /// <summary>
        /// Fired when a node is being removed from its parent node. 
        /// This event is dispatched before the node is removed from the tree. The target of this event is the node being removed.
        /// </summary>
        public static MutationEventType DOMNodeRemoved { get { return Evt<MutationEventType>("DOMNodeRemoved"); } }

        /// <summary>
        /// Fired when a node is being removed from a document, either through direct removal of the Node or removal of a subtree in which it is contained. 
        /// This event is dispatched before the removal takes place. The target of this event is the Node being removed. 
        /// If the Node is being directly removed the DOMNodeRemoved event will fire before the DOMNodeRemovedFromDocument event.
        /// </summary>
        public static MutationEventType DOMNodeRemovedFromDocument { get { return Evt<MutationEventType>("DOMNodeRemovedFromDocument"); } }

        /// <summary>
        /// Fired when a node is being inserted into a document, either through direct insertion of the Node or insertion of a subtree in which it is contained. 
        /// This event is dispatched after the insertion has taken place. The target of this event is the node being inserted. 
        /// If the Node is being directly inserted the DOMNodeInserted event will fire before the DOMNodeInsertedIntoDocument event.
        /// </summary>
        public static MutationEventType DOMNodeInsertedIntoDocument { get { return Evt<MutationEventType>("DOMNodeInsertedIntoDocument"); } }

        /// <summary>
        /// Fired after an Attr has been modified on a node. The target of this event is the Node whose Attr changed. 
        /// The value of attrChange indicates whether the Attr was modified, added, or removed. 
        /// The value of relatedNode indicates the Attr node whose value has been affected. 
        /// It is expected that string based replacement of an Attr value will be viewed as a modification of the 
        /// Attr since its identity does not change. 
        /// Subsequently replacement of the Attr node with a different Attr node is viewed as the removal of the first Attr node and the addition of the second.
        /// </summary>
        public static MutationEventType DOMAttrModified { get { return Evt<MutationEventType>("DOMAttrModified"); } }

        /// <summary>
        /// Fired after CharacterData within a node has been modified but the node itself has not been inserted or deleted. 
        /// This event is also triggered by modifications to PI elements. The target of this event is the CharacterData node.
        /// </summary>
        public static MutationEventType DOMCharacterDataModified { get { return Evt<MutationEventType>("DOMCharacterDataModified"); } }
    }
}
