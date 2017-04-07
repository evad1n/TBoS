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
        float speed = 150f;

        bool top;
        bool left;
        bool right;
        bool bot;

        bool pathfinding = false;
        float pathTimer = 0f;

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
                    Graphics.EnemySlugTextures[0].Width * Game1.PIXEL_SCALE,
                    Graphics.EnemySlugTextures[0].Height * Game1.PIXEL_SCALE
                    );
            }
        }

        public FlyingEnemy(Texture2D texture, Vector2 position) : base(texture, position)
        {
            Texture = texture;
            Position = position;
            direction = Game1.PlayerStats.Player.Position - Position;
            direction.Normalize();
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
            pathTimer += elapsed;

            if(pathTimer > 0.2f)
            {
                pathfinding = false;
            }

            top = CheckCardinalCollision(new Vector2(0, -3)) && velocity.Y < 0;
            bot = CheckCardinalCollision(new Vector2(0, 3)) && velocity.Y > 0;
            left = CheckCardinalCollision(new Vector2(-3, 0)) && velocity.X < 0;
            right = CheckCardinalCollision(new Vector2(3, 0)) && velocity.X > 0;

            if((top || bot || left || right) && pathTimer > 0.2f)
            {
                pathfinding = true;
                pathTimer = 0;
            }


            if(!pathfinding)
            {
                if (Game1.PlayerStats.IsAlive)
                {
                    direction = Game1.PlayerStats.Player.Position - Position;
                }
                else
                {
                    direction = Game1.Camera.Origin - Position;
                }
            }
            else
            {
                //Pathfinding
                if (top)
                {
                    direction.X = 1;
                }
                if (bot)
                {
                    direction.X = 1;
                }
                if (left)
                {
                    direction.Y = -1;
                }
                if (right)
                {
                    direction.Y = 11;
                }
            }

            direction.Normalize();

            velocity = direction * speed;

            if (Position.X + Rect.Width < Game1.Camera.Rect.Left || Position.Y - Rect.Height > Game1.Camera.Rect.Bottom)
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
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            if (CurrentChunk != null)
                Position = CollisionHelper.DetailedCollisionCorrection(previousPosition, Position, Rect, CurrentChunk);
            
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
