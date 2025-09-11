using FlashThunder.Utilities;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;
using RenderingLibrary.Graphics;
using System;
using System.Linq;

namespace FlashThunder.Components
{
    partial class PopupComponent
    {
        private const float MaxFadeTime = 0.5f;
        public float Lifetime { get; set; }
        private float _fadeTime;
        partial void CustomInitialize()
        {
        }

        public void Initialize()
        {
            _fadeTime = Math.Min(MaxFadeTime, Lifetime / 2);
        }

        public void Update(float dt)
        {
            Lifetime -= dt;
            Message.Alpha = (int) float.Lerp(255, 0, Math.Clamp((_fadeTime - Lifetime) / _fadeTime, 0, 1));
        }
    }
}
