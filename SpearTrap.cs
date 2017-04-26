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

        Texture2D trap;
        Rectangle trapRect;

        public new Rectangle Rect
        {
            get
            {
                return RotateRect(new Rectangle((int)Position.X, (int)Position.Y, Game1.hitBox.Width, Game1.hitBox.Height), rotation, origin);
            }
        }

        public SpearTrap(Vector2 position, Vector2 direction) : base (Graphics.Spear, position)
        {
            Position = new Vector2(position.X + 18, position.Y + Game1.TILE_SIZE / 2);
            this.direction = direction;
            startPosition = Position;
            endPosition = startPosition + (direction * texture.Height * 3.5f);

            origin = new Vector2(Position.X + texture.Width / 2, Position.Y + texture.Height / 2);

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

            trapRect = new Rectangle((int)startPosition.X, (int)position.Y + Game1.TILE_SIZE, trap.Width, trap.Height);
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            origin = new Vector2(Position.X + texture.Width / 2, Position.Y + texture.Height / 2);

            bool ready = !attack && !retract && !wait;

            if (attack)
            {
                Position = Move(startPosition, endPosition, attackTimer / 0.2f);
                attackTimer += elapsed;
            }
            else if (retract)
            {
                Position = Move(endPosition, startPosition, retractTimer / 1.9f);
                retractTimer += elapsed;
            }
            else if (wait)
            {
                waitTimer += elapsed;
            }

            if (attackTimer > 0.21f)
            {
                attack = false;
                wait = true;
                attackTimer = 0;
                waitTimer = 0;
            }

            if(waitTimer > 0.5f)
            {
                wait = false;
                waitTimer = 0;
                retract = true;
                retractTimer = 0;
            }

            if(retractTimer > 2f)
            {
                retract = false;
                retractTimer = 0;
                attackTimer = 0;
            }

            //Shoot timer
            if(Vector2.DistanceSquared(Game1.PlayerStats.Player.Position, Position) < 5000 && ready)
            {
                attack = true;
                NotifyNearby(Position);
                attackTimer = 0;
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
            spriteBatch.Draw(Graphics.DebugTexture, destinationRectangle: Rect, color: Color.Red);
        }

        public Vector2 Move(Vector2 start, Vector2 target, float amount)
        {
            float x = start.X + ((target.X - start.X) * amount);
            float y = start.Y + ((target.Y - start.Y) * amount);

            return new Vector2(x, y);
        }

        //He did the math
        public Rectangle RotateRect(Rectangle rect, float rotation, Vector2 origin)
        {
            Rectangle result;

            Vector2 topLeft = new Vector2(rect.X, rect.Y);
            Vector2 topRight = new Vector2(rect.X + rect.Width, rect.Y);
            Vector2 botLeft = new Vector2(rect.X, rect.Y + rect.Height);
            Vector2 botRight = new Vector2(rect.X + rect.Width, rect.Y + rect.Height);

            Matrix TranslateTo = Matrix.CreateTranslation(new Vector3(origin.X, origin.Y, 0));
            Matrix TranslateBack = Matrix.CreateTranslation(new Vector3(-origin.X, -origin.Y, 0));
            Matrix rotate = Matrix.CreateRotationZ(rotation);

            topLeft = Vector2.Transform(topLeft, TranslateBack);
            topLeft = Vector2.Transform(topLeft, rotate);
            topLeft = Vector2.Transform(topLeft, TranslateTo);

            topRight = Vector2.Transform(topRight, TranslateBack);
            topRight = Vector2.Transform(topRight, rotate);
            topRight = Vector2.Transform(topRight, TranslateTo);

            botLeft = Vector2.Transform(botLeft, TranslateBack);
            botLeft = Vector2.Transform(botLeft, rotate);
            botLeft = Vector2.Transform(botLeft, TranslateTo);

            botRight = Vector2.Transform(botRight, TranslateBack);
            botRight = Vector2.Transform(botRight, rotate);
            botRight = Vector2.Transform(botRight, TranslateTo);

            float left = Math.Min(Math.Min(topLeft.X, topRight.X), Math.Min(botLeft.X, botRight.X));
            float top = Math.Min(Math.Min(topLeft.Y, topRight.Y), Math.Min(botLeft.Y, botRight.Y));

            result = new Rectangle((int)left, (int)top, rect.Width, rect.Height);

            return result;
        }

        public void NotifyNearby(Vector2 pos)
        {
            foreach(Entity e in Game1.Player.CurrentChunk.Traps)
            {
                if(e is SpearTrap && Vector2.DistanceSquared(pos, e.Position) < 50000)
                {
                    ((SpearTrap)e).attack = true;
                }
            }
        }
    }
}
