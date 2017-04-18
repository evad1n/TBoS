using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    class SpearTrap : Entity
    {
        float rotation;
        float speed = 100f;
        bool attack = false;
        float attackTimer;
        Vector2 startPosition;
        Vector2 direction;

        public SpearTrap(Vector2 position, Vector2 direction) : base (Graphics.Spear, position)
        {
            this.Position = position;
            this.direction = direction;
            startPosition = position;

            //Calculate spear rotation
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            rotation += MathHelper.ToRadians(90);
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            attackTimer += elapsed;

            if(attackTimer > 2f)
            {
                attack = false;
            }

            //Shoot timer
            if(Vector2.DistanceSquared(Game1.PlayerStats.Player.Position, Position) < 1600)
            {
                attack = true;
                attackTimer = 0;
            }

            if(attack)
            {
                Vector2 move =  Move(Position, startPosition + (direction * texture.Height), speed);
                Position = new Vector2(move.X, move.Y);
            }
            else
            {
                Vector2 move = Move(Position, startPosition, speed);
                Position = new Vector2(move.X, move.Y);
            }
            

        }

        public override void Draw(SpriteBatch spriteBatch, Color color, int depth = 0)
        {

            if (Active)
            {
                if (LockToPixelGrid)
                {
                    Rectangle drawRect = new Rectangle(
                        (int)Math.Round(Position.X / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                        (int)Math.Round((Position.Y + Game1.PIXEL_SCALE) / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                        Texture.Width * Game1.PIXEL_SCALE,
                        Texture.Height * Game1.PIXEL_SCALE
                        );

                    spriteBatch.Draw(Texture, destinationRectangle: Rect, color: color);
                }
                else
                    spriteBatch.Draw(Texture, destinationRectangle: Rect, color: color);
            }
        }

        public Vector2 Move(Vector2 start, Vector2 target, float speed)
        {
            Vector2 v = target - start;
            if (v.Length() != 0)
            {
                v.Normalize();
            }
            return v * speed;
        }
    }
}
