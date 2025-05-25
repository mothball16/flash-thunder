using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nkast.Aether.Physics2D.Dynamics;
namespace FlashThunder.Core.Components
{
    public class BodyComponent
    {
        public Body Value { get; set; }
        public double Mass => Value.Mass;
        
    }
}
