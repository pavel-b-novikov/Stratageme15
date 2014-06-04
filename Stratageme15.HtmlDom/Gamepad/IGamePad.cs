using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratageme15.HtmlDom.Gamepad
{
    /// <summary>
    /// The Gamepad interface defines an individual gamepad or other controller, allowing access to information such as button presses, axis positions, and id.
    /// 
    /// A Gamepad object can be returned in one of two ways: via the gamepad property of the 
    /// Window.gamepadconnected and Window.gamepadconnected events, or by grabbing any position in the array returned by the Navigator.getGamepads function.
    /// </summary>
    public interface IGamePad
    {
        /// <summary>
        /// A string containing identifying information about the controller.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// An integer that is auto-incremented to be unique for each device currently connected to the system.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// A string indicating whether the browser has remapped the controls on the device to a known layout
        /// </summary>
        string Mapping { get; }

        /// <summary>
        /// A boolean indicating whether the gamepad is still connected to the system
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// An array of gamepadButton objects representing the buttons present on the device.
        /// </summary>
        IGamepadButton[] Buttons { get; }

        /// <summary>
        /// Returns an array representing the controls with axes present on the device (e.g. analog thumb sticks)
        /// </summary>
        double[] Axes { get; }

        /// <summary>
        /// A DOMHighResTimeStamp representing the last time the data for this gamepad was updated. 
        /// Note that this property is not currently supported anywhere
        /// </summary>
        double Timestamp { get; }
    }
}
