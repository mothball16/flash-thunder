using FlashThunder.Enums;
using System;

namespace FlashThunder.Events
{
    /// <summary>
    /// An event used to notify listeners that a state change is requested.
    /// </summary>
    public struct LoadScreenEvent
    {
        /// <summary>
        /// The type of the screen to transition to. Use typeof(), or null to unload the screen.
        /// </summary>
        public Type To { get; set; }

        /// <summary>
        /// The layer in the UIManager to overwrite. Screens that shouldn't be simultaneously loaded
        /// (like MenuScreen, TitleScreen) should register as the same layer.
        /// </summary>
        public ScreenLayer Layer { get; set; }

    }
}
