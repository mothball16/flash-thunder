using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DefaultEcs;
using FlashThunder.Gameplay.Components;
using FlashThunder.Gameplay.Resources;
namespace FlashThunder.Extensions
{
    /// <summary>
    /// credit: https://stackoverflow.com/a/4405876/30180794
    /// </summary>
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
            };
    }
}