using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    public class DynamicParticle : Particle
    {
        Vector2 velocity;
        int gravity;

        bool animated;
        int startFrame;
        int currentFrame;
        float frameLength;
        float frameTimer;

        Texture2D[] textures;

        public DynamicParticle(Texture2D texture, Texture2D[] textures, Vector2 position, float lifespan, Vector2 startingVelocity, int gravity = 750, bool animated = false, bool lockToPixelGrid = true, bool active = true) : base(texture, position, lifespan, lockToPixelGrid, active)
        {
            Position = position;
            this.lifespan = lifespan;
            this.gravity = gravity;
            timer = 0;

            this.animated = animated;
            this.textures = textures;

            LockToPixelGrid = lockToPixelGrid;
            Active = active;

            velocity.Y = startingVelocity.Y;
            velocity.X = (float)(Game1.RandomObject.NextDouble() * Game1.RandomObject.Next(-(int)startingVelocity.X, (int)startingVelocity.X));
            if (!animated)
                Texture = textures[Game1.RandomObject.Next(0, textures.Length)];
            else
            {
                startFrame = Game1.RandomObject.Next(0, textures.Length);
                Texture = textures[startFrame];

                frameLength = lifespan / (textures.Length - startFrame);
            }
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Set the X and Y components of the velocity separately.
            velocity.Y = velocity.Y + gravity * elapsed;

            //Move the particle
            Position += velocity * elapsed;

            if (animated)
            {
                frameTimer += elapsed;
                if(frameTimer > frameLength && currentFrame != textures.Length - 1)
                {
                    frameTimer = 0;
                    currentFrame++;

                    Texture = textures[currentFrame];
                }
            }

            base.Update(gameTime);
        }
    }
}
