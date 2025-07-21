namespace FlashThunder.GameLogic.Components;

/// <summary>
/// Calculation for armor damage:
/// hardFinal = Math.Max(hard - hardPen, 0);
/// softFinal = Math.Max(soft / (softPen / 100), 0);
/// afterArmorDmg = (Math.Max(dmg - hardFinal,0) * (1 - softFinal/100))
/// Hard is applied before soft.
/// </summary>
internal readonly record struct Armor(int Hard, int Soft);
internal readonly record struct Health(int CurHealth, int MaxHealth);
internal readonly record struct MaxRange(int Value);
internal readonly record struct Vision(int Value);
