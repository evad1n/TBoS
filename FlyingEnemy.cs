using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    /// <summary>
    /// A hostile entity that roves in a vertical column, and switches direction when it hits a wall. (WIP)
    /// 
    /// By Will Dickinson
    /// </summary>
    class FlyingEnemy : Enemy
    {
        float speed = 120;

        float timer = 0;
        const float pathTimer = 2;
        bool horizontal;

        Vector2 direction;

        //Animation?
        SpriteEffects facing = SpriteEffects.None;
        float animTimer = 0;
        float animSpeed = 0.05f;
        int animFrame = 0;
        int animFramesTotal = 8;

        public new Rectangle Rect
        {
            get
            {
                return new Rectangle(
                    (int)Math.Round(Position.X / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                    (int)Math.Round(Position.Y / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                    Graphics.EnemyFlyerTextures[0].Width * Game1.PIXEL_SCALE,
                    Graphics.EnemyFlyerTextures[0].Height * Game1.PIXEL_SCALE
                    );
            }
        }

        public FlyingEnemy(Vector2 position, bool horizontal) : base(Graphics.EnemyFlyerTextures[0], position)
        {
            Texture = texture;
            Position = position;
            this.horizontal = horizontal;

            timer = pathTimer / 2f;

            if(horizontal)
            {
                direction = new Vector2(-1, 0);
            }
            else
            {
                direction = new Vector2(0, -1);
            }

            velocity = direction * speed;
        }

        /// <summary>
        /// Updates player collision and input states.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="keyboardState">Provides a snapshot of inputs.</param>
        /// <param name="prevKeyboardState">Provides a snapshot of the previous frame's inputs.</param>
        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer += elapsed;

            if (timer > pathTimer)
            {
                direction *= -1;
                timer = 0;
            }
            
            if(horizontal)
            {
                if ((CheckCardinalCollision(new Vector2(-3, 0)) && velocity.X < 0) || (CheckCardinalCollision(new Vector2(3, 0)) && velocity.X > 0))
                {
                    direction *= -1;
                    timer = 0;
                }
            }
            else
            {
                if ((CheckCardinalCollision(new Vector2(0, 3)) && velocity.Y > 0) || (CheckCardinalCollision(new Vector2(0, -3)) && velocity.Y < 0))
                {
                    direction *= -1;
                    timer = 0;
                }
            }

            velocity = direction * speed;

            if (CurrentChunk != null && (Position.X + Rect.Width < Game1.Camera.Rect.Left || Position.Y - Rect.Height > Game1.Camera.Rect.Bottom + 500))
            {
                Active = false;
            }

            //Apply the physics
            ApplyPhysics(gameTime);
        }

        /// <summary>
        /// Applies all the physics (collisions, etc.) for the player.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="keyboardState">Provides a snapshot of inputs.</param>
        public void ApplyPhysics(GameTime gameTime)
        {
            //Save the elapsed time
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Save the previous position
            Vector2 previousPosition = Position;

            if (velocity.X > 0) {
                facing = SpriteEffects.FlipHorizontally;
            } else {
                facing = SpriteEffects.None;
            }

            //Move the player and correct for collisions
            Position += velocity * elapsed;
            
            GetAnimation(elapsed);
        }

        void GetAnimation(float elapsed)
        {
            //Walk animation
            if (animTimer < animSpeed)
            {
                animTimer += elapsed;
                if (animTimer >= animSpeed)
                {
                    animFrame = (animFrame + 1) % animFramesTotal;
                    Texture = Graphics.EnemyFlyerTextures[animFrame];
                    animTimer = 0f;
                }
            }
        }

        //This is necessary for altering the player's hitbox. This method lops off the bottom pixel from the hitbox.
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

                    spriteBatch.Draw(Texture, destinationRectangle: drawRect, color: color, effects: facing);
                }
                else
                    spriteBatch.Draw(Texture, destinationRectangle: Rect, color: color, effects: facing);
            }
        }
    }
}
