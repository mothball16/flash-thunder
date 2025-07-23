using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Camera.Requests;
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
