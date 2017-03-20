using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    public class Player : Entity {

        //PHYSICS
        float speedJump = -1500f; //Speed of the player's initial jump
        float acceleration = 13000f; //how fast the player picks up speed from rest
        float maxFallSpeed = 450f; //max effect of gravity
        float maxSpeed = 1200f; //maximum speed

        float drag = .48f; //speed reduction (need this)

        //Jumping
        bool isJumping;
        bool wasJumping;
        float jumpTime; //The length of the jump
        float maxJumpTime = 0.45f; //how long can the player "sustain" a jump?
        float jumpControlPower = 0.14f;

        bool wallJumped;
        bool canStartJump;

        public bool Grounded;
        public bool Walled;

        public Vector2 velocity;

        public Player(Texture2D texture, Vector2 position) : base(texture, position) {
            Texture = texture;
            Position = position;
        }

        /// <summary>
        /// Updates player collision and input states.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="keyboardState">Provides a snapshot of inputs.</param>
        /// <param name="prevKeyboardState">Provides a snapshot of the previous frame's inputs.</param>
        public void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevKeyboardState) {
            
            //Check collision directions
            Grounded = CheckCardinalCollision(new Vector2(0, 3));
            Walled = CheckCardinalCollision(new Vector2(3, 0)) || CheckCardinalCollision(new Vector2(-3, 0));

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

            if (Walled && !wallJumped)
                maxFallSpeed = 125;
            else
                maxFallSpeed = 450;

            //Apply the physics
            ApplyPhysics(gameTime, keyboardState);

            //Clear the jumping state
            isJumping = false;
        }

        /// <summary>
        /// Applies all the physics (collisions, etc.) for the player.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="keyboardState">Provides a snapshot of inputs.</param>
        void ApplyPhysics(GameTime gameTime, KeyboardState keyboardState) {
            //Save the elapsed time
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Save the previous position
            Vector2 previousPosition = Position;
            //Save the horizontal motion
            float motion = GetXMotionFromInput(keyboardState);

            //Set the X and Y components of the velocity separately.
            velocity.X += motion * acceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + Game1.GRAVITY.Y * elapsed, -maxFallSpeed, maxFallSpeed);

            //Apply tertiary forces
            velocity.Y = DoJump(velocity.Y, gameTime);
            velocity.X *= drag;

            //Clamp the velocity
            velocity.X = MathHelper.Clamp(velocity.X, -maxSpeed, maxSpeed);

            //Move the player and correct for collisions
            Position += velocity * elapsed;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            if (CurrentChunk != null)
                Position = CollisionHelper.DetailedCollisionCorrection(previousPosition, Position, Rect, CurrentChunk);

            //Reset the velocity vector.
            if (Position.X == previousPosition.X)
                velocity.X = 0;
            if (Position.Y == previousPosition.Y) {
                velocity.Y = 0;
                jumpTime = 0.0f;
            }

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
        float DoJump(float velocityY, GameTime gameTime) {
            //Do the following is we're jumping
            if (isJumping) {
                //If we're in the middle of a jump...
                if ((!wasJumping && Grounded) || (Walled && !wallJumped) || jumpTime > 0f) {
                    if (jumpTime == 0f && !wallJumped && !Grounded)
                        wallJumped = true;

                    //If we're just starting a jump and we can jump, start a jump, or we're midair... 
                    if (!(jumpTime == 0f && !canStartJump)) {
                        jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }

                //If we're in the descent of the jump...
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

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                motion += -1f;

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                motion += 1f;

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

        //This is necessary for altering the player's hitbox. This method lops off the bottom pixel from the hitbox.
        public override void Draw(SpriteBatch spriteBatch) {
            if (Active) {
                if (LockToPixelGrid) {
                    Rectangle drawRect = new Rectangle(
                        (int)Math.Round(Position.X / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                        (int)Math.Round((Position.Y + Game1.PIXEL_SCALE) / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                        Texture.Width * Game1.PIXEL_SCALE,
                        Texture.Height * Game1.PIXEL_SCALE
                        );
        
                    spriteBatch.Draw(Texture, drawRect, Color.White);
                } else
                    spriteBatch.Draw(Texture, Rect, Color.White);
            }
        }
    }
}