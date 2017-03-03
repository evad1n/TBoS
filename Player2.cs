using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBondOfStone
{
    class Player2
    {
        Texture2D sprite;

        //The chunk the player is currently "in"
        Chunk chunk;
        public Chunk Chunk { get { return chunk; } }

        //Is the player alive?
        bool isAlive;
        public bool IsAlive {
            get { return isAlive; }
        }

        // Physics state stuff
        Vector2 position;
        public Vector2 Position {
            get { return position; }
            set { position = value; }
        }

        private float previousBottom;

        Vector2 velocity;
        public Vector2 Velocity {
            get { return velocity; }
            set { velocity = value; }
        }

        float acceleration;
        float moveSpeed;
        float dragGround;
        float dragAir;

        float maxJumpTime;
        float jumpLaunchVelocity;
        float gravityAcceleration;
        float maxFallSpeed;
        float jumpControlPower;

        bool grounded;
        public bool Grounded { get { return grounded; } }

        float movement;

        bool isJumping;
        bool wasJumping;
        float jumpTime;

        Rectangle localBounds;
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Bounds.X) + localBounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Bounds.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }

        public Player2(Chunk chunk, Vector2 position, Texture2D texture)
        {
            this.chunk = chunk;
            this.sprite = texture;

            localBounds = sprite.Bounds;
            isAlive = true;
            Velocity = Vector2.Zero;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState) {
            GetInput(keyboardState);

            //ApplyPhysics(gameTime);

            //this is where animation stuff should happen

            movement = 0f;
            isJumping = false;
        }

        void GetInput(KeyboardState keyboardState)
        {
            if (Math.Abs(movement) < 0.5f)
                movement = 0f;


            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                movement = 1f;
            
        }
    }
}
