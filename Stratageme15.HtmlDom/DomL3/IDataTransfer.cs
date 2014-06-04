namespace Stratageme15.HtmlDom.DomL3
{
    public enum DropEffect
    {
        /// <summary>
        ///  A copy of the source item is made at the new location.
        /// </summary>
        Copy,
        /// <summary>
        /// An item is moved to a new location.
        /// </summary>
        Move,
        /// <summary>
        /// A link is established to the source at the new location.
        /// </summary>
        Link,
        /// <summary>
        /// The item may not be dropped.
        /// </summary>
        None
    }
    public enum  EffectAllowed
    {
        /// <summary>
        /// the default value when the effect has not been set, equivalent to all.
        /// </summary>
        Uninitialized,
        /// <summary>
        /// A copy of the source item may be made at the new location.
        /// </summary>
        Copy,
        /// <summary>
        /// An item may be moved to a new location.
        /// </summary>
        Move,
        /// <summary>
        /// A link may be established to the source at the new location.
        /// </summary>
        Link,
        /// <summary>
        ///  A copy or link operation is permitted.
        /// </summary>
        CopyLink,
        /// <summary>
        /// A copy or move operation is permitted.
        /// </summary>
        CopyMove,
        /// <summary>
        ///  A link or move operation is permitted.
        /// </summary>
        LinkMove,
        /// <summary>
        /// All operations are permitted.
        /// </summary>
        All,
        /// <summary>
        /// the item may not be dropped.
        /// </summary>
        None
    }
    /// <summary>
    /// The DataTransfer object is used to hold the data that is being dragged during a drag and drop operation. 
    /// It may hold one or more data items, each of one or more data types.
    /// </summary>
    public interface IDataTransfer
    {
        /// <summary>
        /// Contains a list of all the local files available on the data transfer. 
        /// If the drag operation doesn't involve dragging files, this property is an empty list. An invalid index access on the FileList specified by this property will return null
        /// </summary>
        IInputFile[] Files { get; }

        /// <summary>
        /// Holds a list of the format types of the data that is stored for the first item, in the same order the data was added. An empty list will be returned if no data was added.
        /// </summary>
        string[] Types { get; }

        /// <summary>
        /// The actual effect that will be used, and should always be one of the possible values of effectAllowed
        /// </summary>
        DropEffect DropEffect { get; }

        /// <summary>
        /// Specifies the effects that are allowed for this drag.
        ///  You may set this in the dragstart event to set the desired effects for the source, and within the dragenter and dragover events to set the desired effects for the target.
        /// </summary>
        EffectAllowed EffectAllowed { get; }

        /// <summary>
        /// Remove the data associated with a given type. The type argument is optional. 
        /// If the type is empty or not specified, the data associated with all types is removed. 
        /// If data for the specified type does not exist, or the data transfer contains no data, this method will have no effect.
        /// </summary>
        /// <param name="type"></param>
        void ClearData(string type = null);

        /// <summary>
        /// Retrieves the data for a given type, or an empty string if data for that type does not exist or the data transfer contains no data.
        /// A security error will occur if you attempt to retrieve data during a drag that was set from a different domain, or the caller would otherwise not have access to. This data will only be available once a drop occurs during the drop event.
        /// </summary>
        /// <param name="type">The type of data to retrieve</param>
        /// <returns></returns>
        string GetData(string type);

        /// <summary>
        /// Set the data for a given type. If data for the type does not exist, it is added at the end, such that the last item in the types list will be the new format. If data for the type already exists, the existing data is replaced in the same position. That is, the order of the types list is not changed when replacing data of the same type.
        /// </summary>
        /// <param name="type">The type of data to add.</param>
        /// <param name="data">The data to add.</param>
        void SetData(string type, string data);

        /// <summary>
        /// Set the image to be used for dragging if a custom one is desired. Most of the time, this would not be set, as a default image is created from the node that was dragged.
        /// If the node is an HTML img element, an HTML canvas element or a XUL image element, the image data is used. Otherwise, image should be a visible node and the drag image will be created from this. If image is null, any custom drag image is cleared and the default is used instead.
        /// The coordinates specify the offset into the image where the mouse cursor should be. To center the image, for instance, use values that are half the width and height of the image.
        /// </summary>
        /// <param name="image">An element to use as the drag feedback image</param>
        /// <param name="x">Horizontal offset within the image.</param>
        /// <param name="y">Vertical offset within the image.</param>
        void SetDragImage(IElement image, long x, long y);
    }
}
