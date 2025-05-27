using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Gameplay.Components;
namespace FlashThunder.Gameplay.Systems
{
    internal class DebugSystem : ISystem<float>
    {
        private readonly World _world;
        private readonly EntitySet _entitySet;
        public bool IsEnabled { get; set; }

        public DebugSystem(World world)
        {
            _world = world;
            _entitySet = world.GetEntities()
                .With<GridPosComponent>()
                .AsSet();
        }


        public void Update(float dt)
        {
            foreach(ref readonly Entity e in _entitySet.GetEntities())
            {
                Console.WriteLine(e.Get<GridPosComponent>());
            }
        }
        public void Dispose() { }
    }
}
