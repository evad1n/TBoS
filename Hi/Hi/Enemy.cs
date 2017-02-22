using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics;

namespace Hi
{
    class Enemy
    {
        Fixture leftFixture;
        Fixture rightFixture;
        Fixture center;

        private int speed = 5;
        private int direction = -1;

        public Enemy(World w, Vector2 position, Vector2 leftPosition, Vector2 rightPosition)
        {
            leftFixture.UserData = "bound";
            rightFixture.UserData = "bound";
            center.OnCollision += HandleCollision;
        }
        public void Update(GameTime gameTime)
        {
            center.Body.LinearVelocity = new Vector2(speed * direction, 0);
        }

        public void Draw(SpriteBatch sb, Texture2D enemyTexture)
        {
            sb.Draw(enemyTexture, center.Body.Position, Color.White);
        }

        public bool HandleCollision(Fixture f1, Fixture f2, Contact contact)
        {
            if((string)f2.UserData == "bound")
            {
                direction *= -1;
            }
            return true;
        }
    }
}
