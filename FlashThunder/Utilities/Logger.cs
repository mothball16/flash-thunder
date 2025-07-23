using fennecs;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FlashThunder.Utilities;

/// <summary>
/// Very poorly coded logger for more organized printing to console.
/// To be scrapped once an actual debugging system is made.
/// </summary>
internal static class Logger
{
    [MethodImpl(MethodImplOptions.NoInlining)]  //This will prevent inlining by the complier.
    public static string GetCaller()
        => new StackTrace(1, false).GetFrame(1).GetMethod().DeclaringType.Name;
    
    public static void Error(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[{GetCaller()}] {msg}");
    }

    public static void Warn(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[{GetCaller()}] {msg}");
    }

    public static void Print(string msg)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"[{GetCaller()}] {msg}");
    }

    public static void Confirm(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[{GetCaller()}] {msg}");
    }

    public static void PrintEntityComponents(Entity e)
    {
        var sb = new StringBuilder($"Entity {e.GetHashCode()}\n- - -\n");
        foreach (var comp in e.Components)
        {
            sb.AppendLine($"> {comp.Type}, {comp.Box.Value}");
        }
        sb.AppendLine($"- - -");
        Print(sb.ToString());
    }
}
