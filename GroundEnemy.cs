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
    /// A hostile entity that walks along a flat surface, and switches direction when it hits a wall or gap. (WIP)
    /// 
    /// By Will Dickinson
    /// </summary>
    class GroundEnemy : Enemy
    {
        float speed = 50f;
        int direction = 1;

        Chunk nextChunk;
        Rectangle gapRect;
        Rectangle wallRect;
        int yOffset;

        //Animation?
        SpriteEffects facing = SpriteEffects.None;
        float walkingTimer = 0;
        float walkFrameSpeed = 0.05f;
        int walkFrame = 0;
        int walkFramesTotal = 5;

        public bool Grounded;

        public new Rectangle Rect
        {
            get
            {
                yOffset = (Game1.TILE_PIXEL_SIZE - Graphics.EnemySlugTextures[0].Height) * 3;

                return new Rectangle(
                    (int)Math.Round(Position.X / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                    (int)Math.Round(Position.Y / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                    Graphics.EnemySlugTextures[0].Width * Game1.PIXEL_SCALE,
                    Graphics.EnemySlugTextures[0].Height * Game1.PIXEL_SCALE
                    );
            }
        }

        public GroundEnemy(Texture2D texture, Vector2 position) : base(texture, position)
        {
            Texture = texture;
            Position = position;
        }

        /// <summary>
        /// Updates player collision and input states.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="keyboardState">Provides a snapshot of inputs.</param>
        /// <param name="prevKeyboardState">Provides a snapshot of the previous frame's inputs.</param>
        public override void Update(GameTime gameTime)
        {
            //Update pathfinding colliders
            gapRect = new Rectangle(Rect.X + (Game1.TILE_SIZE * direction), Rect.Y - (yOffset) + Game1.TILE_SIZE, Game1.TILE_SIZE, Game1.TILE_SIZE);
            wallRect = new Rectangle(Rect.X + (Game1.TILE_SIZE/3 * direction), Rect.Y - yOffset, Game1.TILE_SIZE, Game1.TILE_SIZE);
            nextChunk = Game1.Generator.GetEntityChunkID(gapRect);

            //Check collision directions
            Grounded = CheckCardinalCollision(new Vector2(0, 3));

            //Check for pathfinding (gaps and walls)
            if ((!CollisionHelper.IsCollidingWithChunk(nextChunk, gapRect) || CollisionHelper.IsCollidingWithChunk(nextChunk, wallRect)) && Grounded)
            {
                direction *= -1;
            }

            velocity.X = speed * direction;

            base.Update(gameTime);

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

            //Set the X and Y components of the velocity separately.
            velocity.Y = velocity.Y + Game1.GRAVITY.Y * elapsed;

            if(velocity.X > 0)
            {
                facing = SpriteEffects.FlipHorizontally;
            }
            else
            {
                facing = SpriteEffects.None;
            }

            //Move the player and correct for collisions
            Position += velocity * elapsed;

            if (CurrentChunk != null)
                Position = CollisionHelper.DetailedCollisionCorrection(previousPosition, Position, Rect, CurrentChunk);


            GetAnimation(elapsed);
        }

        void GetAnimation(float elapsed)
        {
            //Walk animation
            if (walkingTimer < walkFrameSpeed)
            {
                walkingTimer += elapsed;
                if (walkingTimer >= walkFrameSpeed)
                {
                    walkFrame = (walkFrame + 1) % walkFramesTotal;
                    Texture = Graphics.EnemySlugTextures[walkFrame];
                    walkingTimer = 0f;
                }
            }
        }

        //This is necessary for altering the player's hitbox. This method lops off the bottom pixel from the hitbox.
        public override void Draw(SpriteBatch spriteBatch, Color color, int depth = 0)
        {
            //debug
            //spriteBatch.Draw(Graphics.Tiles_gold[0], gapRect, Color.White);
            //spriteBatch.Draw(Graphics.Tiles_gold[0], wallRect, Color.White);

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

                    spriteBatch.Draw(Texture, destinationRectangle: Rect, color: color, effects: facing);
                }
                else
                    spriteBatch.Draw(Texture, destinationRectangle: Rect, color: color, effects: facing);
            }
        }
    }
}
