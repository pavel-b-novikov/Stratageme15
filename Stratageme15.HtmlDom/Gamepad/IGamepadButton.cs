namespace Stratageme15.HtmlDom.Gamepad
{
    /// <summary>
    /// The GamepadButton interface  defines an individual button of a gamepad 
    /// or other controller, allowing access to the current state of different 
    /// types of button available on the control device
    /// </summary>
    public interface IGamepadButton
    {
        /// <summary>
        /// A double value used to enable represent the current state of analog buttons, such as the triggers on many modern gamepads. 
        /// The values are normalized to the range 0.0 — 1.0, with 0.0 representing a button that is not pressed, and 1.0 representing a button that is fully pressed
        /// </summary>
        double Value { get; }

        /// <summary>
        /// A boolean indicating whether the button is currently pressed (true) or unpressed (false)
        /// </summary>
        bool Pressed { get; }
    }
}
