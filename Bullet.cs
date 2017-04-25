﻿using Microsoft.Xna.Framework;
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
        public float rotation;
        float rotationSpeed;
        public bool facingLeft = false;
        public float stuckRotation;

        public TurretEnemy parent;
        public bool bounce;
        float airTime = 0f;
        Vector2 target;       

        bool Grounded;
        bool Right;
        bool Left;
        bool Ceiling;
        public bool stuck = false;
        public bool sticky = false;

        public Vector2 velocity;
        public Vector2 previousVelocity;
        public Vector2 origin;
        public Vector2 relativePosition;

        Projectile type;
       

        public new Rectangle Rect
        {
            get
            {        
                Rectangle hitRect = RotateRect(Position, rotation, origin);
                switch (type)
                {
                    case Projectile.Sawblade:
                        return new Rectangle(
                        (int)Position.X,
                        (int)Position.Y,
                        texture.Width * Game1.PIXEL_SCALE,
                        texture.Height * Game1.PIXEL_SCALE
                        );
                    case Projectile.Spear:
                        return hitRect;
                    case Projectile.Arrow:
                        return hitRect;
                }

                return new Rectangle(
                (int)Math.Round(Position.X / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                (int)Math.Round(Position.Y / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                texture.Width * Game1.PIXEL_SCALE,
                texture.Height * Game1.PIXEL_SCALE
                );
            }
        }


        public Bullet(TurretEnemy parent, Vector2 target, float speed, Texture2D texture, Vector2 position, float rotationSpeed, Projectile type, bool bounce = false, int spread = 0) : base(texture, position)
        {
            this.speed = speed;
            this.parent = parent;
            this.target = target;
            Texture = texture;
            Position = position;
            this.bounce = bounce;
            this.rotationSpeed = rotationSpeed;
            this.type = type; 

            if(target == Vector2.Zero)
            {
                target = Game1.PlayerStats.Player.Position;

                //Attempt to throw from a position that doesn't result in hitting the ground everytime
                int xDir = Math.Sign(target.X - Position.X);
                Position += new Vector2(5 * xDir, 5);

                //Account for spread
                target += new Vector2(Game1.RandomObject.Next(spread));

                //Calculate bullet rotation
                Vector2 dir = target - position;
                dir.Normalize();
                rotation = (float)Math.Atan2(dir.Y, dir.X);
                rotation += MathHelper.ToRadians(90);


                target = Move(Position, target, speed);
            }
            else
            {
                switch (type)
                {
                    case Projectile.Sawblade:
                        Position = new Vector2(Position.X, Position.Y - texture.Height);
                        Position += (target * 20);
                        break;
                    case Projectile.Spear:
                        break;
                    case Projectile.Arrow:
                        Position += (target * 5);
                        break;
                }

                //Calculate bullet rotation
                rotation = (float)Math.Atan2(target.Y, target.X);
                rotation += MathHelper.ToRadians(90);

                target *= speed;

                //Account for spread
                target += new Vector2(Game1.RandomObject.Next(spread));
            }

            velocity = target;
        }
        
        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            origin = new Vector2(Position.X + texture.Width / 2, Position.Y + texture.Height / 2);

            rotation += elapsed * rotationSpeed * Math.Sign(velocity.X);

            if (CurrentChunk != null && (Position.Y - Rect.Height < Game1.Camera.Rect.Top || Position.X - Rect.Width > Game1.Camera.Rect.Right || Position.X + Rect.Width < Game1.Camera.Rect.Left || Position.Y - Rect.Height > Game1.Camera.Rect.Bottom + 500))
            {
                Active = false;
            }


            if (bounce)
            {
                //Check collision directions
                Grounded = CheckCardinalCollision(new Vector2(0, 3));
                Right = CheckCardinalCollision(new Vector2(3, 0));
                Left = CheckCardinalCollision(new Vector2(-3, 0));
                Ceiling = CheckCardinalCollision(new Vector2(0, -3));

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
                    velocity = new Vector2(velocity.X, -(Game1.GRAVITY.Y * airTime/2));
                    Game1.Camera.ScreenShake(velocity.Y * 3, velocity.Y);
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

                if (e != null && e != parent && !stuck)
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
                velocity.Y += Game1.GRAVITY.Y * elapsed;
            }

            //Move the player and correct for collisions
            Position += velocity * elapsed;

            if(bounce)
            {
                //stuff that could fix sawblade sucking
            }
            else
            {
                //Check for collisions with level geometry
                if (CollisionHelper.IsCollidingWithChunk(CurrentChunk, Rect))
                {
                    stuck = true;
                    velocity = Vector2.Zero;
                }
            }


        }

        public override void Draw(SpriteBatch spriteBatch, Color color, int depth = 0)
        {
            if(sticky)
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

                    drawRect.X += drawRect.Width / 2;
                    drawRect.Y += drawRect.Height / 2;

                    spriteBatch.Draw(texture: Texture, destinationRectangle: drawRect, color: color, origin: new Vector2(texture.Width/2, texture.Height/2), rotation: rotation);
                }
                else
                    spriteBatch.Draw(texture: Texture, destinationRectangle: Rect, color: color, origin: new Vector2(texture.Width / 2, texture.Height / 2), rotation: rotation);
            }
            spriteBatch.Draw(Graphics.DebugTexture, destinationRectangle: Rect, color: Color.Red);
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

        public void Flip()
        {
            float r = MathHelper.ToDegrees(rotation);
            r = 180 - r;
            rotation += MathHelper.ToRadians(2 * r);
        }

        public Rectangle RotateRect(Vector2 pos, float rotation, Vector2 origin)
        {
            Rectangle result;

            Vector2 source = new Vector2(pos.X - origin.X, pos.Y - origin.Y);

            double x = (Math.Cos(rotation) * source.X) + (-Math.Sin(rotation) * source.Y);
            double y = (Math.Sin(rotation) * source.X) + (Math.Cos(rotation) * source.Y);

            source = new Vector2((float)x, (float)y);
            source = new Vector2(source.X + (float)(Game1.hitBox.Width * Math.Cos(rotation)), source.Y + (float)(Game1.hitBox.Height * Math.Sin(rotation)));

            result = new Rectangle((int)source.X, (int)source.Y, Game1.hitBox.Width, Game1.hitBox.Height);
            return result;
        }
    }
}
