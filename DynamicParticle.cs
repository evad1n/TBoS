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

        Texture2D[] textures;
        Vector2 velocity;

        static int gravity = 500;

        public DynamicParticle(Texture2D texture, Texture2D[] textures, Vector2 position, float lifespan, Vector2 startingVelocity, bool lockToPixelGrid = true, bool active = true) : base(texture, position, lifespan, lockToPixelGrid, active)
        {
            Position = position;
            this.lifespan = lifespan;
            timer = 0;

            LockToPixelGrid = lockToPixelGrid;
            Active = active;

            velocity.Y = startingVelocity.Y;
            velocity.X = (float)(Game1.RandomObject.NextDouble() * Game1.RandomObject.Next(-(int)startingVelocity.X, (int)startingVelocity.X));
            Texture = textures[Game1.RandomObject.Next(0, textures.Length)];
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Set the X and Y components of the velocity separately.
            velocity.Y = velocity.Y + gravity * elapsed;

            //Move the particle
            Position += velocity * elapsed;

            base.Update(gameTime);
        }
    }
}
