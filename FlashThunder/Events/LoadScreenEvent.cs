using FlashThunder.Enums;
using System.Collections.Generic;

namespace FlashThunder.Events;
/// <summary>
/// An event used to notify listeners that a state change is requested.
/// </summary>
internal readonly record struct LoadScreenEvent(Screen Screen, Dictionary<string, object> Deps = null);