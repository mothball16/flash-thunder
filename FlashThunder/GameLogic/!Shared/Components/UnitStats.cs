namespace FlashThunder.GameLogic.Components;

/// <summary>
/// Calculation for armor damage:
/// hardFinal = Math.Max(hard - hardPen, 0);
/// softFinal = Math.Max(soft / (softPen / 100), 0);
/// afterArmorDmg = (Math.Max(dmg - hardFinal,0) * (1 - softFinal/100))
/// Hard is applied before soft.
/// </summary>
internal struct Armor
{
    public int Hard { get; set; }
    public int Soft { get; set; }
}
internal struct Health
{
    public int CurHealth { get; set; }
    public int MaxHealth { get; set; }
}

internal struct Vision
{
    public int Value { get; set; }
}
