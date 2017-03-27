using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    /// <summary>
    /// The player class. Allows the player to move throughout the world and interprets collisions and entity interactions.
    /// 
    /// By Dom Liotti
    /// </summary>
    public class Player : Entity {

        //PHYSICS
        float speedJump = -1500f; //Speed of the player's initial jump
        float acceleration = 13000f; //how fast the player picks up speed from rest
        float maxFallSpeed = 450f; //max effect of gravity
        float maxSpeed = 1200f; //maximum speed

        float drag = .48f; //speed reduction (need this)

        float goombaForce = -1000;

        //Particle production
        float particleFrequency = 0.065f;
		float particleLifetime = 4.5f;
        float particleTimer;
        List<Particle> particles = new List<Particle>();

        //Animation?
        SpriteEffects facing = SpriteEffects.None;
        float walkingTimer = 0;
        float walkFrameSpeed = 0.05f;
        int walkFrame = 0;
        int walkFramesTotal = 4;

        //Jumping
        public bool isJumping;
        bool wasJumping;
        float jumpTime; //The length of the jump
        float maxJumpTime = 0.45f; //how long can the player "sustain" a jump?
        float jumpControlPower = 0.14f;

        bool WalljumpGivesXVelocity = false;
        float wallJumpXVelocity = 750;

        float airTime;

        bool wallJumped;
        public bool canStartJump;

        public bool Alive;
        public bool Grounded;
        public bool Walled;
        bool walledRight;
        bool walledLeft;

        public Vector2 velocity;

        public new Rectangle Rect
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    Graphics.PlayerTextures[0].Width * Game1.PIXEL_SCALE,
                    Graphics.PlayerTextures[0].Height * Game1.PIXEL_SCALE
                    );
            }
        }

        public Player(Texture2D texture, Vector2 position) : base(texture, position) {
            Texture = texture;
            Position = position;

            particleTimer = 0;
        }

        /// <summary>
        /// Updates player collision and input states.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="keyboardState">Provides a snapshot of inputs.</param>
        /// <param name="prevKeyboardState">Provides a snapshot of the previous frame's inputs.</param>
        public void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevKeyboardState) {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Alive = Game1.PlayerStats.IsAlive;

            //Check collision directions
            Grounded = CheckCardinalCollision(new Vector2(0, 3));
            walledLeft = CheckCardinalCollision(new Vector2(-3, 0));
            walledRight = CheckCardinalCollision(new Vector2(3, 0));
            Walled = walledLeft || walledRight;

            //Determine canStartJump states (Yes, this is necessary)
            isJumping = (keyboardState.IsKeyDown(Keys.Space) || 
                keyboardState.IsKeyDown(Keys.W) || 
                keyboardState.IsKeyDown(Keys.Up));

            canStartJump = (keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.W) ||
                keyboardState.IsKeyDown(Keys.Up)) &&
                !(prevKeyboardState.IsKeyDown(Keys.Space) ||
                prevKeyboardState.IsKeyDown(Keys.W) ||
                prevKeyboardState.IsKeyDown(Keys.Up)) &&
                !wallJumped;

            ResolveDynamicEntityCollisions();

            if (!Alive) {
                    Walled = false;
                    canStartJump = false;
            }

            if (Walled && !wallJumped)
                maxFallSpeed = 125;
            else
                maxFallSpeed = 450;

            //Apply the physics
            ApplyPhysics(gameTime, keyboardState);

            //Clear the jumping state
            isJumping = false;

            //Create particles if necessary
            particleTimer += elapsed;
            if (particleTimer >= particleFrequency) {
                bool canSpawnBottom = 
                    CollisionHelper.IsCollidingWithChunk(CurrentChunk, new Rectangle(Rect.X, Rect.Center.Y, 1, Rect.Height/2 + 1)) &&
                    CollisionHelper.IsCollidingWithChunk(CurrentChunk, new Rectangle(Rect.Right, Rect.Center.Y, 1, Rect.Height/2 + 1));
                bool canSpawnLeft = 
                    CollisionHelper.IsCollidingWithChunk(CurrentChunk, new Rectangle(Rect.Center.X - (Rect.Width/2 + 1), Rect.Top, Rect.Width/2 + 1, 1)) &&
                    CollisionHelper.IsCollidingWithChunk(CurrentChunk, new Rectangle(Rect.Center.X - (Rect.Width/2 + 1), Rect.Bottom, Rect.Width/2 + 1, 1));
                bool canSpawnRight = 
                    CollisionHelper.IsCollidingWithChunk(CurrentChunk, new Rectangle(Rect.Center.X, Rect.Top, Rect.Width/2 + 1, 1)) &&
                    CollisionHelper.IsCollidingWithChunk(CurrentChunk, new Rectangle(Rect.Center.X, Rect.Bottom, Rect.Width/2 + 1, 1));

                if (Grounded && canSpawnBottom && velocity.X != 0)
                    particles.Add(new Particle(Graphics.Effect_PlayerParticlesBottom[Game1.RandomObject.Next(0, Graphics.Effect_PlayerParticlesBottom.Length)], new Vector2(Position.X, Position.Y + Game1.PIXEL_SCALE * 7), particleLifetime + (float)Game1.RandomObject.NextDouble() * particleLifetime));
                else if (walledLeft && canSpawnLeft && velocity.Y != 0)
                    particles.Add(new Particle(Graphics.Effect_PlayerParticlesLeft[Game1.RandomObject.Next(0, Graphics.Effect_PlayerParticlesLeft.Length)], new Vector2(Position.X - Game1.PIXEL_SCALE * 2, Position.Y), particleLifetime + (float)Game1.RandomObject.NextDouble() * particleLifetime));
                else if (walledRight && canSpawnRight && velocity.Y != 0)
                    particles.Add(new Particle(Graphics.Effect_PlayerParticlesRight[Game1.RandomObject.Next(0, Graphics.Effect_PlayerParticlesRight.Length)], new Vector2(Position.X + Game1.PIXEL_SCALE * 4, Position.Y), particleLifetime + (float)Game1.RandomObject.NextDouble() * particleLifetime));
                particleTimer = 0;
            }
            
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Update(gameTime);

                if (!particles[i].Active)
                    particles.Remove(particles[i]);
            }

            if (Grounded && velocity.Y == maxFallSpeed) {
                Game1.Camera.ScreenShake(airTime * 3, airTime);
                airTime = 0;
            }

            if (!Grounded && !Walled)
                airTime += elapsed;
            else
                airTime = 0;

            //Collect coins if necessary
            if (CurrentChunk != null && CurrentChunk.Entities.Count > 0) {
                foreach (Entity e in CurrentChunk.Entities) {
                    if(e is CoinPickup) {
                        CoinPickup c = (CoinPickup)e;

                        if (c != null && Rect.Intersects(c.Rect))
                            c.Collect();
                    }else if(e is Spike) {
                        Spike s = (Spike)e;

                        //If the player is touching this spike...
                        if (s != null && Rect.Intersects(s.HitRect)) {
                            //Take damage
                            Game1.PlayerStats.TakeDamage(1, s);
                        }
                    }
                }
            }
        }

        public void ResolveDynamicEntityCollisions()
        {
            foreach (Entity e in Game1.dynamicEntities) {
                //Collisions for Ground Enemies
                if (e is GroundEnemy) {
                    GroundEnemy g = (GroundEnemy)e;

                    if (g != null && Rect.Intersects(g.Rect)) {
                        if (g.Active) {
                            if (Position.Y < g.Position.Y && velocity.Y > 0) {
                                g.Kill();
                                KnockBack(new Vector2(0, goombaForce));
                            } else
                                Game1.PlayerStats.TakeDamage(1, g);
                        }
                    }
                } else if (e is JumpingEnemy) {
                    JumpingEnemy j = (JumpingEnemy)e;

                    if (j != null && Rect.Intersects(j.Rect))
                    {
                        if (j.Active)
                        {
                            if (Position.Y < j.Position.Y && velocity.Y > 0)
                            {
                                j.Kill();
                                KnockBack(new Vector2(0, goombaForce));
                            }
                            else
                                Game1.PlayerStats.TakeDamage(1, j);
                        }
                    }
                }
                else if (e is FlyingEnemy)
                {
                    FlyingEnemy f = (FlyingEnemy)e;

                    if (f != null && Rect.Intersects(f.Rect))
                    {
                        if (f.Active)
                        {
                            if (Position.Y < f.Position.Y && velocity.Y > 0)
                            {
                                f.Kill();
                                KnockBack(new Vector2(0, goombaForce));
                            }
                            else
                                Game1.PlayerStats.TakeDamage(1, f);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Applies all the physics (collisions, etc.) for the player.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="keyboardState">Provides a snapshot of inputs.</param>
        public void ApplyPhysics(GameTime gameTime, KeyboardState keyboardState) {
            //Save the elapsed time
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float motion = 0f;

            //Save the previous position
            Vector2 previousPosition = Position;    
            //Save the horizontal motion
            if(Alive)
                motion = GetXMotionFromInput(keyboardState);

            //Set the X and Y components of the velocity separately.
            velocity.X += motion * acceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + Game1.GRAVITY.Y * elapsed, goombaForce, maxFallSpeed);

            //Apply tertiary forces
            velocity.Y = DoJump(velocity.Y, gameTime);
            velocity.X *= drag;

            //Clamp the velocity
            velocity.X = MathHelper.Clamp(velocity.X, -maxSpeed, maxSpeed);

            //Move the player and correct for collisions
            Position += velocity * elapsed;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            if (CurrentChunk != null && Game1.PlayerStats.IsAlive)
                Position = CollisionHelper.DetailedCollisionCorrection(previousPosition, Position, Rect, CurrentChunk);

            //Reset the velocity vector.
            if (Position.X == previousPosition.X)
                velocity.X = 0;
            if (Position.Y == previousPosition.Y) {
                velocity.Y = 0;
                jumpTime = 0.0f;
            }

            GetAnimation(elapsed);

            //set the grounded-walled state
            if (Grounded || !Walled)
                wallJumped = false;
        }

        /// <summary>
        /// Return a float for the velocity of a jumping/falling character.
        /// </summary>
        /// <param name="velocityY">The Y velocity to modify.</param>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns></returns>
        public float DoJump(float velocityY, GameTime gameTime) {
            //Do the following is we're jumping
            if (isJumping) {
                //If we're in the middle of a jump...
                if ((!wasJumping && Grounded) || (Walled && !wallJumped) || jumpTime > 0f) {
                    if (jumpTime == 0f && !wallJumped && !Grounded)
                    {
                        if (WalljumpGivesXVelocity)
                        {
                            if (walledLeft)
                                velocity.X = wallJumpXVelocity;
                            else if (walledRight)
                                velocity.X = -wallJumpXVelocity;
                        }
                        wallJumped = true;
                    }

                    //If we're just starting a jump or we're midair... 
                    if (jumpTime != 0f || canStartJump) {
                        jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }

                //If we're in the ascent of the jump...
                if (0f < jumpTime && jumpTime <= maxJumpTime) {
                    velocityY = speedJump * (1.0f - (float)Math.Pow(jumpTime / maxJumpTime, jumpControlPower));
                } else
                    jumpTime = 0f;
            } else
                jumpTime = 0f;

            wasJumping = isJumping;

            return velocityY;
        }

        //Returns -1, 0, or 1 depending on the X input state.
        public float GetXMotionFromInput(KeyboardState keyboardState) {
            float motion = 0f;

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left)) {
                motion += -1f;

                if(!Walled)
                    facing = SpriteEffects.FlipHorizontally;
            }

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                motion += 1f;

                if(!Walled)
                    facing = SpriteEffects.None;
            }

            return motion;
        }
        
        Vector2 Move(Vector2 motion) {
            return Position + motion;
        }

        /// <summary>
        /// checks whether the player is "next to" a collidable surface.
        /// </summary>
        /// <param name="offset">The direction of the check.</param>
        /// <returns>Boolean is true when the player's rect offset by "offset" is colliding with the level.</returns>
        public bool CheckCardinalCollision(Vector2 offset) {
            if (CurrentChunk != null) {
                Rectangle check = Rect;
                check.Offset(offset);
                return CollisionHelper.IsCollidingWithChunk(CurrentChunk, check);
            } else
                return false;
        }

        void GetAnimation(float elapsed) {
            if(!Alive)
                Texture = Graphics.PlayerTextures[6];
            //Jump animation
            else if (!Grounded && !Walled) {
                if (velocity.Y > -450 && velocity.Y < -100)
                    Texture = Graphics.PlayerTextures[2];
                else if (velocity.Y > -100 && velocity.Y < -50)
                    Texture = Graphics.PlayerTextures[3];
                else if (velocity.Y > -50 && velocity.Y < 50)
                    Texture = Graphics.PlayerTextures[4];
                else if (velocity.Y > 50 && velocity.Y < 100)
                    Texture = Graphics.PlayerTextures[5];
                else if (velocity.Y > -100 && velocity.Y < 450)
                    Texture = Graphics.PlayerTextures[6];
            }
            //Walk animation
            else if (Grounded && !Walled && velocity.X != 0) {
                if(walkingTimer < walkFrameSpeed)
                {
                    walkingTimer += elapsed;
                    if(walkingTimer >= walkFrameSpeed)
                    {
                        walkFrame = (walkFrame + 1) % walkFramesTotal;
                        Texture = Graphics.PlayerWalkTextures[walkFrame];
                        walkingTimer = 0f;
                    }
                }
            }
            //Idle
            else if(Grounded && velocity.X == 0) {
                Texture = Graphics.PlayerTextures[0];
            }
            //Walled texture
            else if (!Grounded && Walled) {
                    Texture = Graphics.PlayerTextures[1];
            }
        }

        //This is necessary for altering the player's hitbox. This method lops off the bottom pixel from the hitbox.
        public override void Draw(SpriteBatch spriteBatch, Color color) {
            if (Active) {
                if (LockToPixelGrid) {
                    Rectangle drawRect = new Rectangle(
                        (int)Math.Round(Position.X / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                        (int)Math.Round((Position.Y + Game1.PIXEL_SCALE) / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                        Texture.Width * Game1.PIXEL_SCALE,
                        Texture.Height * Game1.PIXEL_SCALE
                        );
        
                    spriteBatch.Draw(Texture, destinationRectangle: drawRect, color: color, effects: facing);
                } else
                    spriteBatch.Draw(Texture, destinationRectangle: Rect, color: color, effects: facing);
            }

            foreach (Particle p in particles)
                p.Draw(spriteBatch, Color.White);
        }

        public void KnockBack(Vector2 boom)
        {
            velocity.X = boom.X;
            velocity.Y = boom.Y;
            Game1.Camera.ScreenShake(4f, 0.3f);
        }
    }
}