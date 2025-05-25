using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Core.Components;
namespace FlashThunder.Core.Systems
{
    internal class EntityCountingSystem : ISystem<float>
    {
        private readonly World _world;
        private readonly EntitySet _entitySet;
        public bool IsEnabled { get; set; }

        public EntityCountingSystem(World world)
        {
            _world = world;
            _entitySet = world.GetEntities()
                .With<MapPosComponent>()
                .AsSet();
        }


        public void Update(float dt)
        {
            foreach(ref readonly Entity e in _entitySet.GetEntities())
            {
                Console.WriteLine(e.Get<MapPosComponent>());
            }
        }
        public void Dispose() { }
    }
}
