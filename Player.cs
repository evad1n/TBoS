using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TheBondOfStone
{
    class Player : Entity
    {
        private int health;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Player(World world, Shape shape, Vector2 position, Texture2D texture)
            :base (world, shape, position, texture)
        {

        }
    }
}
