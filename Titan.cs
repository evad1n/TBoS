using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    class Titan : Entity {

        float speed;
        float animSpeed = 0.5f;
        float animTimer = 0f;
        int animFrame = 0;
        int numFrames;

        public bool FacingRight;

        Texture2D[] titanTextures;

        public Titan(Texture2D texture, Vector2 position, float speed, Texture2D[] titanTextures, bool facingRight) : base(texture, position, true, true) {
            Texture = texture;
            Position = position;

            this.speed = speed;

            this.titanTextures = titanTextures;
            animFrame = 0;
            numFrames = titanTextures.Length;

            FacingRight = facingRight;
        }

        public void Update(GameTime gameTime) {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Move across the screen
            Position = new Vector2(Position.X + speed, Position.Y);

            //Animate
            GetAnimation(elapsed);
        }

        void GetAnimation(float elapsed) {
            //Walk animation
            if (animTimer < animSpeed) {
                animTimer += elapsed;
                if (animTimer >= animSpeed) {
                    animFrame = (animFrame + 1) % numFrames;
                    Texture = titanTextures[animFrame];
                    animTimer = 0f;
                }
            }
        }
    }
}
