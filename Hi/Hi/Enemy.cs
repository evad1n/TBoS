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
        Body left;
        Body right;
        Body center;

        private int speed = 10;
        private int direction = -1;

        public Enemy(World w, Vector2 position, Vector2 leftPosition, Vector2 rightPosition, Texture2D tex)
        {
            center = BodyFactory.CreateRoundedRectangle(w, tex.Width/5, tex.Height/5, 10, 20, 1, 1, "enemy");
            left = BodyFactory.CreateEdge(w, new Vector2(leftPosition.X - 1, leftPosition.Y - 200), new Vector2 (leftPosition.X + 1, leftPosition.Y + 200), "bound");
            right = BodyFactory.CreateEdge(w, new Vector2(rightPosition.X - 1, rightPosition.Y - 200), new Vector2(rightPosition.X + 1, rightPosition.Y + 200), "bound");

            center.BodyType = BodyType.Dynamic;
            left.BodyType = BodyType.Static;
            right.BodyType = BodyType.Static;

            center.Position = position;
            left.Position = leftPosition;
            right.Position = rightPosition;
            center.OnCollision += HandleCollision;
        }
        public void Update(GameTime gameTime)
        {
            center.LinearVelocity = new Vector2(speed * direction, 0);
        }

        public void Draw(SpriteBatch sb, Texture2D enemyTexture, Texture2D boundsTexture)
        {
            sb.Draw(enemyTexture, position: center.Position, color: Color.White, scale: new Vector2(0.2f));
            sb.Draw(boundsTexture, position: left.Position, color: Color.White, scale: new Vector2(0.2f));
            sb.Draw(boundsTexture, position: right.Position, color: Color.White, scale: new Vector2(0.2f));
        }

        public bool HandleCollision(Fixture f1, Fixture f2, Contact contact)
        {
            if((string)f2.Body.UserData == "bound")
            {
                direction *= -1;
            }
            return true;
        }
    }
}
