namespace FlashThunder.Enums;

public enum ScreenLayer
{
    /// <summary>
    /// extra layer in case any are necessary
    /// </summary>
    Back = 10,

    /// <summary>
    /// for the main screen BGs
    /// </summary>
    Primary = 20,

    /// <summary>
    /// for active HUDs like unit selection
    /// </summary>
    HUD = 30,

    /// <summary>
    /// for alerts and stuff
    /// </summary>
    Popup = 40,

    /// <summary>
    /// for overlays
    /// </summary>
    Overlay = 50,

    /// <summary>
    /// for stuff that absolutely must be on top
    /// </summary>
    System = 60
}
