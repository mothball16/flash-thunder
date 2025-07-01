using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FlashThunder.Managers;
using FlashThunder.ECSGameLogic.Resources;
using FlashThunder.ECSGameLogic.Components;
using MonoGame.Shapes;
using System;
using FlashThunder.ECSGameLogic.Components.UnitStats;
using FlashThunder.Extensions;
using FlashThunder.Defs;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Units
{
    /// <summary>
    /// Creates a marker underneath all controlled entities.
    /// </summary>
    internal sealed class ControlledEntityMarkerSystem : ISystem<float>
    {
        private readonly World _world;
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Any entities controlled and with a position on the grid should fall here.
        /// </summary>
        private readonly EntitySet _entitySet;

        public ControlledEntityMarkerSystem(World world)
        {
            _world = world;
            _entitySet = world.GetEntities()
                .With<GridPosComponent>()
                .With<ControlledComponent>()
                .AsSet();
        }

        public void Update(float dt)
        {
            var env = _world.Get<EnvironmentResource>();
            foreach (Entity e in _entitySet.GetEntities())
            {
                 //guard clause -- if the entity is controlled (but not by us), skip it
                if (env.FocusedTeam != e.Get<ControlledComponent>().Owner)
                    continue;

                var pos = e.Get<GridPosComponent>();
                _world.RequestSpawn(EntityID.ControlMarker, pos.X, pos.Y);

            }
        }

        public void Dispose()
        {
        }
    }
}