using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBondOfStone
{
    class Camera : Camera2D
    {
        Player target;

        public Camera(GraphicsDevice graphicsDevice, Player target) : base(graphicsDevice)
        {
            this.target = target;
            
        }
    }
}
