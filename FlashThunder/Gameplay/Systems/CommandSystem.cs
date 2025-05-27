using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Enums;
using FlashThunder.Gameplay.Components;
using Microsoft.Xna.Framework;
namespace FlashThunder.Gameplay.Systems
{
    /// <summary>
    /// Modifies the intents of all controlled units.
    /// </summary>
    internal class CommandSystem : ISystem<float>
    {
        private readonly World _world;
        private readonly EntitySet _entitySet;

        public bool IsEnabled { get; set; }

        public CommandSystem(World world)
        {
            _world = world;
            _entitySet = world.GetEntities()
                .With<ControlledComponent>()
                .With<GridPosComponent>()
                .AsSet();
        }


        public void Update(float dt)
        {

            foreach (ref readonly Entity e in _entitySet.GetEntities())
            {

            }
        }
        public void Dispose() { }
    }
}
