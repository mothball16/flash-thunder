using System.Text.Json.Serialization;

namespace FlashThunder.GameLogic.Components;

internal struct SmoothScalable
{
    public float ScaleCurrent { get; set; }
    public float ScaleTarget { get; set; }
    public float ScaleResponse { get; set; }

    [JsonConstructor]
    public SmoothScalable(float scaleCurrent = 1f, float scaleTarget = 1f, float scaleResponse = 10f)
    {
        ScaleCurrent = scaleCurrent;
        ScaleTarget = scaleTarget;
        ScaleResponse = scaleResponse;
    }

    public override string ToString()
    {
        return $"SmoothScalable(ScaleCurrent: {ScaleCurrent}, ScaleTarget: {ScaleTarget}, ScaleResponse: {ScaleResponse})";
    }
}