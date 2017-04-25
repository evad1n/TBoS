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

        bool attack = false;
        bool retract = false;
        bool wait = false;
        float attackTimer;
        float retractTimer;
        float waitTimer;

        Vector2 startPosition;
        Vector2 endPosition;
        Vector2 direction;
        Vector2 origin;
        Vector2 point;

        Texture2D trap;
        Rectangle trapRect;

        public new Rectangle Rect
        {
            get
            {
                return new Rectangle(
                (int)Math.Round(point.X / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                (int)Math.Round(point.Y / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                Game1.hitBox.Width,
                Game1.hitBox.Height
                );
            }
        }

        public SpearTrap(Vector2 position, Vector2 direction) : base (Graphics.Spear, position)
        {
            Position = new Vector2(position.X + 18, position.Y + Game1.TILE_SIZE / 2);
            this.direction = direction;
            startPosition = Position;
            endPosition = startPosition + (direction * texture.Height * 3.5f);

            //Calculate spear rotation
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            rotation += MathHelper.ToRadians(90);

            //Get rect and rotation for trap texture
            if (MathHelper.ToDegrees(rotation) == 180 || MathHelper.ToDegrees(rotation) == 360)
            {
                trap = Graphics.SpearTrap[0];
            }
            else
            {
                trap = Graphics.SpearTrap[1];
            }

            origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            trapRect = new Rectangle((int)startPosition.X, (int)position.Y + Game1.TILE_SIZE, trap.Width, trap.Height);
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            origin = new Vector2(Position.X + texture.Width / 2, Position.Y + texture.Height / 2);
            point = RotatePoint(Position, rotation, origin);

            bool ready = !attack && !retract && !wait;

            if(attackTimer > 0.2f)
            {
                attack = false;
                wait = true;
                attackTimer = 0;
            }

            if(waitTimer > 0.5f)
            {
                wait = false;
                waitTimer = 0;
                retract = true;
            }

            if(retractTimer > 2f)
            {
                retract = false;
                retractTimer = 0;
            }

            //Shoot timer
            if(Vector2.DistanceSquared(Game1.PlayerStats.Player.Position, Position) < 5000 && ready)
            {
                attack = true;
                attackTimer = 0;
            }

            if(attack)
            {
                Position = Move(startPosition, endPosition, attackTimer / 0.2f);
                attackTimer += elapsed;
            }
            else if(retract)
            {
                Position = Move(endPosition, startPosition, retractTimer / 2f);
                retractTimer += elapsed;
            }
            else if(wait)
            {
                waitTimer += elapsed;
            }
           
        }

        public override void Draw(SpriteBatch spriteBatch, Color color, int depth = 0)
        {
            Rectangle r = new Rectangle(trapRect.X, trapRect.Y, trapRect.Width * Game1.PIXEL_SCALE, trapRect.Height * Game1.PIXEL_SCALE);

            spriteBatch.Draw(trap, destinationRectangle: r, color: color, rotation: rotation);
        }

        public void DrawSpear(SpriteBatch spriteBatch, Color color, int depth = 0)
        {
            if (Active)
            {
                if (LockToPixelGrid)
                {
                    Rectangle drawRect = new Rectangle(
                        (int)Position.X,
                        (int)Position.Y,
                        Texture.Width * Game1.PIXEL_SCALE,
                        Texture.Height * Game1.PIXEL_SCALE
                        );

                    spriteBatch.Draw(Texture, destinationRectangle: drawRect, color: color, scale: new Vector2(20), origin: Vector2.Zero, rotation: rotation);
                }
                else
                    spriteBatch.Draw(Texture, destinationRectangle: Rect, color: color, scale: new Vector2(20), origin: Vector2.Zero, rotation: rotation);
            }
            spriteBatch.Draw(Graphics.DebugTexture, destinationRectangle: Rect, color: Color.Red, origin: Vector2.Zero, rotation: rotation);
        }

        public Vector2 Move(Vector2 start, Vector2 target, float amount)
        {
            float x = start.X + ((target.X - start.X) * amount);
            float y = start.Y + ((target.Y - start.Y) * amount);

            return new Vector2(x, y);
        }

        public Vector2 RotatePoint(Vector2 source, float rotation, Vector2 origin)
        {
            source = new Vector2(source.X - origin.X, source.Y - origin.Y);

            double x = (Math.Cos(rotation) * source.X) + (-Math.Sin(rotation) * source.Y);
            double y = (Math.Sin(rotation) * source.X) + (-Math.Cos(rotation) * source.Y);

            Vector2 result = new Vector2((float)x, (float)y);
            result = new Vector2(result.X + origin.X, result.Y + origin.Y);

            return result;
        }
    }
}
