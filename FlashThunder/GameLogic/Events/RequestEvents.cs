using fennecs;
using FlashThunder.GameLogic.Movement.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Events;

internal readonly record struct MovableTilesUpdateRequest;
internal readonly record struct NextTurnRequest;
internal readonly record struct SpawnPrefabRequest(
    string Name,
    Action<Entity> Callback = null,
    GridPosition? Position = null,
    string Team = null
    );