using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBondOfStone
{
    //Basic walking enemy that will patrol back and forth across a set path
    class GroundEnemy : Enemy
    {
        private Fixture leftFixture;
        private Fixture rightFixture;
        private bool rightCollision;
        private bool leftCollision;

        public Fixture RightFixture
        {
            get { return rightFixture; }
            set { rightFixture = value; }
        }

        public Fixture LeftFixture
        {
            get { return leftFixture; }
            set { leftFixture = value; }
        }


        public GroundEnemy(World world, Shape shape, Vector2 position, Texture2D texture, Shape headShape)
            :base (world, shape, position, texture, headShape)
        {
            leftFixture.OnCollision += HandleLeftCollision;
            rightFixture.OnCollision += HandleRightCollision;
        }

        public void Update()
        {          
            //pseudocode for AI
        }

        public bool HandleLeftCollision(Fixture f1, Fixture f2, Contact contact)
        {
            leftCollision = true;
            return true;
        }

        public bool HandleRightCollision(Fixture f1, Fixture f2, Contact contact)
        {
            rightCollision = true;
            return true;
        }
    }
}
