namespace Stratageme15.HtmlDom.Gamepad
{
    public interface IGamepadEvent : IEvent
    {
        /// <summary>
        /// Returns a Gamepad object, providing access to the associated gamepad data for the event fired
        /// </summary>
        IGamePad Gamepad { get; }
    }
}
