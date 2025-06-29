using FlashThunder.Enums;
using FlashThunder.Managers;
using Gum.Wireframe;
using System;

namespace FlashThunder.Events
{
    /// <summary>
    /// An event used to notify listeners that a state change is requested.
    /// </summary>
    public struct LoadScreenEvent
    {
        /// <summary>
        /// The ScreenFactory should return the initialized screen with its dependencies loaded 
        /// (via this lambda)
        /// </summary>
        public UIElementFactory ScreenFactory { get; set; }

        /// <summary>
        /// The layer in the UIManager to overwrite. Screens that shouldn't be simultaneously loaded
        /// (like MenuScreen, TitleScreen) should register as the same layer.
        /// </summary>
        public ScreenLayer Layer { get; set; }
    }
}
