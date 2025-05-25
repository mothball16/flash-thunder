using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Core.Components;
using FlashThunder.Enums;
using nkast.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
namespace FlashThunder.Core.Systems
{
    internal class CommandSystem : ISystem<float>
    {
        private readonly GameContext _context;
        private readonly EntitySet _entitySet;

        public bool IsEnabled { get; set; }

        public CommandSystem(GameContext context)
        {
            _context = context;
            _entitySet = context.World.GetEntities()
                .With<ControlledComponent>()
                .With<MapPosComponent>()
                .With<BodyComponent>()
                .AsSet();
        }


        public void Update(float dt)
        {

            foreach (ref readonly Entity e in _entitySet.GetEntities())
            {
                var body = e.Get<BodyComponent>().Value;


                Vector2 moveVel = Vector2.Zero;
                if (_context.Input.IsActive(PlayerAction.MoveLeft))
                    moveVel += new Vector2(-1, 0);
                if (_context.Input.IsActive(PlayerAction.MoveRight))
                    moveVel += new Vector2(1, 0);
                if (_context.Input.JustActivated(PlayerAction.Jump))
                    body.ApplyLinearImpulse(new Vector2(0, -10));

                body.LinearVelocity = new Vector2(moveVel.X, body.LinearVelocity.Y);
            }
        }
        public void Dispose() { }
    }
}
