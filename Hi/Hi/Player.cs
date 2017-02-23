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
    class Player
    {
        Body center;
        KeyboardState kb;

        private int speed = 100;

        public Player(World w, Vector2 position, Texture2D tex)
        {
            center = BodyFactory.CreateRoundedRectangle(w, tex.Width/5, tex.Height/5, 10, 20, 1, 1, "player");
            center.Position = position;

            center.OnCollision += HandleCollision;
            center.BodyType = BodyType.Dynamic;
        }
        public void Update(GameTime gameTime)
        {
            kb = Keyboard.GetState();

            if(kb.IsKeyDown(Keys.A))
            {
                center.LinearVelocity = new Vector2(-speed, 0);
            }
            if (kb.IsKeyDown(Keys.D))
            {
                center.LinearVelocity = new Vector2(speed, 0);
            }
            if (kb.IsKeyDown(Keys.Space))
            {
                center.LinearVelocity = new Vector2(0, 10);
            }
            if (kb.IsKeyDown(Keys.W))
            {
                center.LinearVelocity = new Vector2(0, 10);
            }
        }

        public void Draw(SpriteBatch sb, Texture2D playerTexture)
        {
            sb.Draw(playerTexture, position: center.Position, color: Color.White, scale: new Vector2(0.2f));
        }

        public bool HandleCollision(Fixture f1, Fixture f2, Contact contact)
        {
            if ((string)f2.UserData == "bound")
            {
                return false;
            }
            return true;
        }
    }
}