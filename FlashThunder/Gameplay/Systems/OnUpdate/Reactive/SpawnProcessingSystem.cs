using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcrew.MonoGame._2D_Camera;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Core;
using FlashThunder.Defs;
using FlashThunder.Gameplay.Components;
using FlashThunder.Gameplay.Resources;
namespace FlashThunder.Gameplay.Systems.OnUpdate.Debugging
{
    internal class SpawnProcessingSystem : ISystem<float>
    {
        private readonly World _world;
        private EntitySet _entitySet;
        private EntityFactory _entityFactory;

        public bool IsEnabled { get; set; }
        
       

        public SpawnProcessingSystem(World world, EntityFactory factory)
        {
            _world = world;
            _entityFactory = factory;
            _entitySet = _world.GetEntities()
                .With<SpawnRequestComponent>()
                .AsSet();
        }

        public void Update(float dt)
        {
            foreach(ref readonly Entity request in _entitySet.GetEntities())
            {
                var requestInfo = request.Get<SpawnRequestComponent>();
                var entity = _entityFactory.CreateEntity(requestInfo.EntityID, requestInfo.X, requestInfo.Y);

                if (requestInfo.EntityID == EntityID.TestEntity)
                {
                    Console.WriteLine(
                        $"{requestInfo.EntityID} was requested @ {requestInfo.X}, {requestInfo.Y}");

                }

                request.Dispose();
            }
           
        }
        public void Dispose() { 

        }
    }
}
