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
        float rotation;
        float rotationSpeed;

        public TurretEnemy parent;
        public bool bounce;
        Vector2 direction;
        float airTime = 0f;
        Vector2 target;       

        bool Grounded;
        bool Right;
        bool Left;
        bool Ceiling;
        public bool stuck = false;

        public Vector2 velocity;
        public Vector2 previousVelocity;
        public Vector2 origin;
        public Vector2 relativePosition;

        public new Rectangle Rect
        {
            get
            {
                origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    (Texture.Height / 4) * Game1.PIXEL_SCALE,
                    (Texture.Height / 4) * Game1.PIXEL_SCALE
                    );
            }
        }


        public Bullet(TurretEnemy parent, Vector2 target, float speed, Texture2D texture, Vector2 position, float rotationSpeed, bool bounce = false, int spread = 0) : base(texture, position)
        {
            this.speed = speed;
            this.parent = parent;
            this.target = target;
            Texture = texture;
            Position = position;
            this.bounce = bounce;
            this.rotationSpeed = rotationSpeed;

            //Account for spread
            target += new Vector2(Game1.RandomObject.Next(spread));

            //Calculate direction;
            direction = Move(Position, target, speed);
            velocity = direction;

            //Calculate bullet rotation
            Vector2 dir = target - position;
            dir.Normalize();
            rotation = (float)Math.Atan2(dir.Y, dir.X);
            rotation += MathHelper.ToRadians(90);
        }
        
        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            rotation += elapsed * rotationSpeed * Math.Sign(velocity.X);

            if (Position.X + Rect.Width < Game1.Camera.Rect.Left || Position.Y - Rect.Height > Game1.Camera.Rect.Bottom)
            {
                Active = false;
            }

            //Check collision directions
            Grounded = CheckCardinalCollision(new Vector2(0, 1));
            Right = CheckCardinalCollision(new Vector2(1, 0));
            Left = CheckCardinalCollision(new Vector2(-1, 0));
            Ceiling = CheckCardinalCollision(new Vector2(0, -1));

            if (bounce)
            {
                //At the vertex set airtime to 0
                if (previousVelocity.Y < 0 && (velocity.Y > 0 || velocity.Y == 0))
                {
                    airTime = 0;
                }

                //Increment airtime when not grounded
                if (!Grounded)
                    airTime += elapsed;

                if (Grounded && velocity.Y < 0)
                {
                    velocity = new Vector2(velocity.X, -(Game1.GRAVITY.Y * airTime));
                    //Game1.Camera.ScreenShake(velocity.Y * 3, velocity.Y);
                    airTime = 0;
                }
                else if(Ceiling && velocity.Y > 0)
                {
                    velocity = new Vector2(velocity.X, -velocity.Y);
                }
                else if (Right && velocity.X > 0)
                {
                    velocity = new Vector2(-velocity.X, velocity.Y);
                }
                else if (Left && velocity.X < 0)
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

            //Apply gravity
            if(bounce)
            {
                velocity.Y = velocity.Y + Game1.GRAVITY.Y * elapsed;
            }

            //Move the player and correct for collisions
            Position += velocity * elapsed;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            if(bounce)
            {

            }
            else
            {
                //Check for collisions with level geometry
                if (CollisionHelper.IsCollidingWithChunk(CurrentChunk, Rect))
                {
                    stuck = true;
                    velocity = Vector2.Zero;
                }

                if (!stuck)
                {
                    if (CurrentChunk != null && Game1.PlayerStats.IsAlive)
                        Position = CollisionHelper.DetailedCollisionCorrection(previousPosition, Position, Rect, CurrentChunk);
                }
            }


        }

        public override void Draw(SpriteBatch spriteBatch, Color color, int depth = 0)
        {
            if(relativePosition != null)
            {
                color = Game1.PlayerStats.invulnColor;
            }
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

                    spriteBatch.Draw(texture: Texture, destinationRectangle: drawRect, color: color, origin: origin, rotation: rotation, scale: new Vector2(0.2f));
                }
                else
                    spriteBatch.Draw(texture: Texture, destinationRectangle: Rect, color: color, origin: origin, rotation: rotation, scale: new Vector2(0.2f));
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
