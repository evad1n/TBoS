using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheBondOfStone
{
    public class PhysicsObject
    {
        public Body Body { get; private set; }

        public Vector2 Position
        {
            get { return UnitConvert.ToScreen(Body.Position); }
            set { Body.Position = UnitConvert.ToWorld(value); }
        }

        private Vector2 size;
        public Vector2 Size
        {
            get { return UnitConvert.ToScreen(size); }
            set { size = UnitConvert.ToWorld(value); }
        }

        public PhysicsObject(Vector2 size, float mass, string tag = "", BodyType bType = BodyType.Static)
        {
            Body = BodyFactory.CreateRectangle(Game1.world, size.X * UnitConvert.pixelToUnit, size.Y * UnitConvert.pixelToUnit, 1);
            Body.BodyType = bType;
            Body.UserData = tag;

            Size = size;

            Body.Friction = 0f;
        }

        public PhysicsObject(float width, float height, float mass, string tag = "", BodyType bType = BodyType.Static) {
            Body = BodyFactory.CreateRoundedRectangle(Game1.world, width * UnitConvert.pixelToUnit, height * UnitConvert.pixelToUnit, 2 * UnitConvert.pixelToUnit, 2 * UnitConvert.pixelToUnit, 1, 1f, tag);
            Body.BodyType = bType;
            Body.UserData = tag;

            Size = new Vector2(width, height);

            Body.Friction = 0f;
        }

        public PhysicsObject(float radius, float mass, string tag = "", BodyType bType = BodyType.Static) {
            Body = BodyFactory.CreateCircle(Game1.world, radius, 1f, tag);
            Body.BodyType = bType;
            Body.UserData = tag;

            Size = new Vector2(radius * 2, radius * 2);

            Body.Friction = 0f;
        }
    }
}
