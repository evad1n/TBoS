using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    class Particle : Entity {

        float timer;
        float lifespan;

        public Particle(Texture2D texture, Vector2 position, float lifespan, bool lockToPixelGrid = true, bool active = true) : base(texture, position, lockToPixelGrid, active)
        {
            Texture = texture;
            Position = position;
            this.lifespan = lifespan;
            timer = 0;

            LockToPixelGrid = lockToPixelGrid;
            Active = active;
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Active)
            {
                timer += elapsed;
                if (timer >= lifespan)
                {
                    Active = false;
                }
            }
        }
    }
}
