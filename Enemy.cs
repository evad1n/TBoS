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
    class Enemy : Entity
    {
        private Fixture headFixture;
        private Body headBody;
        private bool armored;

        public bool Armored
        {
            get { return armored; }
            set { armored = value; }
        }

        public Fixture HeadFixture
        {
            get { return headFixture; }
            set { headFixture = value; }
        }

        public Enemy(World world, Shape shape, Vector2 position, Texture2D texture, Shape headShape)
            :base (world, shape, position, texture, "enemy")
        {
            headBody = new Body(world);
            headBody.CreateFixture(headShape, "enemyHead");
        }
    }
}
