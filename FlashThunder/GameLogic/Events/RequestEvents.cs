using fennecs;
using FlashThunder.ECSGameLogic.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Events;

internal readonly record struct NextTurnRequest;

internal readonly record struct SpawnPrefabRequest(
    string Name,
    Action<Entity> Callback = null,
    GridPosition? Position = null,
    string Team = null
    );

public enum CamOperationType
{
    Smooth,
    Immediate,
}

public enum CamOffsetType
{
    Absolute,
    Relative,
    RelativeSingleFrame
}
internal readonly record struct CamTranslationRequest(
    float X = 0,
    float Y = 0,
    CamOperationType CamOp = CamOperationType.Smooth,
    CamOffsetType OffsetType = CamOffsetType.Relative
    );