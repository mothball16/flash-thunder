using FlashThunder.Events;
using FlashThunder.Interfaces;
using FlashThunder.Managers;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

namespace FlashThunder.Screens;

partial class GameScreen
{
    public GameScreen(IEventSubscriber subscriber)
    {
        subscriber.Subscribe<EntityCountChangedEvent>(OnEntityCountChanged);
    }

    private void OnEntityCountChanged(EntityCountChangedEvent msg)
    {
        UnitCount.Text = $"Active units: {msg.Count}";
    }
    partial void CustomInitialize()
    {
        
    }
}
