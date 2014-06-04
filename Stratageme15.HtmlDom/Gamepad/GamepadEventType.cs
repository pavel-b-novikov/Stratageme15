namespace Stratageme15.HtmlDom.Gamepad
{
    class GamepadEventType : BaseEventType
    {
        internal GamepadEventType(string type) : base(type)
        {
        }
        /// <summary>
        /// A gamepad has been connected
        /// </summary>
        public static GamepadEventType GamepadConnected { get { return Evt<GamepadEventType>("gamepadconnected"); } }

        /// <summary>
        /// A gamepad has been disconnected.
        /// </summary>
        public static GamepadEventType GamepadDisconnected { get { return Evt<GamepadEventType>("gamepaddisconnected"); } }
    }
}
