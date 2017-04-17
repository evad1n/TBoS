using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    public class HealthPickup : Entity
    {
        int value;

        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }

        Texture2D[] textures;

        float animationTimer = 0;
        float animationFrameSpeed = 0.05f;
        int animationFrame = 0;
        int animationFramesTotal = 30;

        public HealthPickup(Texture2D texture, Vector2 position, int value) : base(texture, position) {
            Texture = texture;
            Position = position;

            Value = value;

            textures = new Texture2D[animationFramesTotal];

            for(int i = 0; i < Graphics.PickupTexture_Health.Length; i++)
                textures[i] = Graphics.PickupTexture_Health[i];
            
            for(int i = Graphics.PickupTexture_Health.Length; i < textures.Length; i++)
                textures[i] = Graphics.PickupTexture_Health[0];
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer < animationFrameSpeed)
            {
                animationTimer += elapsed;
                if (animationTimer >= animationFrameSpeed)
                {
                    animationFrame = (animationFrame + 1) % animationFramesTotal;
                    Texture = textures[animationFrame];
                    animationTimer = 0f;
                }
            }
        }

        public void Collect()
        {
            if (Active)
            {
                Game1.PlayerStats.TakeDamage(-value);

                Active = false;
            }
        }
    }
}
