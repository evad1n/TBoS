using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    public class Bullet : Entity
    {
        float speed;
        Vector2 target;
        float rotation;
        TurretEnemy parent;
        bool bounce;
        Vector2 direction;
        float airTime = 0f;

        public bool Grounded;
        public bool Walled;
        bool walledRight;
        bool walledLeft;

        public Vector2 velocity;
        public Vector2 previousVelocity;

        public Bullet(TurretEnemy parent, Vector2 target, float speed, Texture2D texture, Vector2 position, bool bounce = false, int spread = 0) : base(texture, position)
        {
            this.speed = speed;
            this.parent = parent;
            Texture = texture;
            Position = position;
            this.bounce = bounce;

            //Account for spread
            target += new Vector2(Game1.RandomObject.Next(spread));

            //Calculate direction;
            direction = Move(Position, target, speed);

            //Calculate bullet rotation
            Vector2 dir = target - position;
            rotation = (float)Math.Atan2(dir.Y, dir.X);
        }
        
        public void Update(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Position.X + Rect.Width < Game1.Camera.Rect.Left || Position.Y - Rect.Height > Game1.Camera.Rect.Bottom)
            {
                Active = false;
            }

            //Check collision directions
            Grounded = CheckCardinalCollision(new Vector2(0, 3));
            walledLeft = CheckCardinalCollision(new Vector2(-3, 0));
            walledRight = CheckCardinalCollision(new Vector2(3, 0));
            Walled = walledLeft || walledRight;

            velocity = direction;

            if (Grounded && velocity.Y > 0)
            {
                Game1.Camera.ScreenShake(airTime * 3, airTime);
                airTime = 0;
            }

            //Bounce
            if (previousVelocity.Y < 0 && (velocity.Y > 0 || velocity.Y == 0))
            {
                airTime = 0;
            }


            if (!Grounded && !Walled)
                airTime += elapsed;
            else if (Grounded && !Walled)
            {
                //Once you hit the ground
                if (bounce)
                {
                    velocity = new Vector2(velocity.X, -(Game1.GRAVITY.Y * airTime));
                }
                airTime = 0;
            }
            else
            {
                airTime = 0;
            }

            if (bounce)
            {
                if (CheckCardinalCollision(new Vector2(2, 0)) && velocity.X > 0)
                {
                    velocity = new Vector2(-velocity.X, velocity.Y);
                }
                else if (CheckCardinalCollision(new Vector2(-2, 0)) && velocity.X < 0)
                {
                    velocity = new Vector2(-velocity.X, velocity.Y);
                }

                //Check for collisions with enemies
                Enemy e = CollisionHelper.IsCollidingWithEnemy(CurrentChunk, Rect);

                if (e != null && e != parent)
                {
                    e.Kill();
                }
            }
            else
            {
                //Check for collisions with level geometry
                if (CollisionHelper.IsCollidingWithChunk(CurrentChunk, Rect))
                {
                    Active = false;
                }

                //Check for collisions with enemies
                Enemy e = CollisionHelper.IsCollidingWithEnemy(CurrentChunk, Rect);

                if (e != null && e != parent)
                {
                    e.Kill();
                    Kill();
                }
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

            //Save the previous velocity
            previousVelocity = velocity;

            //Save the previous position
            Vector2 previousPosition = Position;

            //Move the player and correct for collisions
            Position += velocity * elapsed;

        }

        public override void Draw(SpriteBatch spriteBatch, Color color, int depth = 0)
        {
            //If this is active, draw it.
            if (Active)
            {
                //We can "lock" entities to the virtual pixel grid (looks pretty nice)
                if (LockToPixelGrid)
                {
                    Rectangle drawRect = new Rectangle(
                        (int)Math.Round(Position.X / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                        (int)Math.Round(Position.Y / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                        Texture.Width * Game1.PIXEL_SCALE,
                        Texture.Height * Game1.PIXEL_SCALE
                        );

                    spriteBatch.Draw(texture: Texture, destinationRectangle: drawRect, color: color, rotation: rotation, scale: new Vector2(0.2f));
                }
                else
                    spriteBatch.Draw(texture: Texture, destinationRectangle: Rect, color: color, rotation: rotation, scale: new Vector2(0.2f));
            }
        }

        public bool CheckCardinalCollision(Vector2 offset)
        {
            if (CurrentChunk != null)
            {
                Rectangle check = Rect;
                check.Offset(offset);
                return CollisionHelper.IsCollidingWithChunk(CurrentChunk, check);
            }
            else
                return false;
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

        public void Kill()
        {
            //Bullet explosion or something

            Active = false;
        }
    }
}
