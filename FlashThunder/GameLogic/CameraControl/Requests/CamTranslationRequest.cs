namespace FlashThunder.GameLogic.CameraControl.Requests;
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
