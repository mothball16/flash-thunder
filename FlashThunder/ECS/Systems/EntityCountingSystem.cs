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
        private readonly GameContext _context;
        private readonly EntitySet _entitySet;
        public bool IsEnabled { get; set; }

        public EntityCountingSystem(GameContext context)
        {
            _context = context;
            _entitySet = context.Entities.GetEntities()
                .With<PositionComponent>()
                .With<VelocityComponent>()
                .AsSet();
        }


        public void Update(float dt)
        {
            foreach(ref readonly Entity e in _entitySet.GetEntities())
            {
                Console.WriteLine(e.Get<PositionComponent>());
                Console.WriteLine(e.Get<VelocityComponent>());
            }
        }
        public void Dispose() { }
    }
}
