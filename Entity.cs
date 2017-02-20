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
    //Class for entities that will appear on the screen and collide
    abstract class Entity
    {
        private Texture2D texture;
        private Fixture fixture;
        private Body body;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Fixture Fixture
        {
            get { return fixture; }
            set { fixture = value; }
        }

        public Vector2 Position
        {
            get { return fixture.Body.Position; }
            set { fixture.Body.Position = value; }
        }


        public Entity(World world, Shape shape, Vector2 position, Texture2D texture)
        {
            this.texture = texture;
            body = new Body(world);
            fixture = body.CreateFixture(shape);
            fixture.Body.Position = position;
        }
    }
}
