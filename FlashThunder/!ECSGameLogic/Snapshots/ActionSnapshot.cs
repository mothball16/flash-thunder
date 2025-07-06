using System.Collections.Generic;
using FlashThunder.Enums;

namespace FlashThunder.Snapshots;

public readonly record struct ActionSnapshot(HashSet<GameAction> Active);