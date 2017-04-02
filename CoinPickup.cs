using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    /// <summary>
    /// A coin which can be picked up and adds to the player's multiplier progress.
    /// 
    /// By Dom Liotti and Noah Bock.
    /// </summary>
    class CoinPickup : Entity {

        int value;

        public int Value {
            get { return value; }
            set { this.value = value; }
        }

        float animationTimer = 0;
        float animationFrameSpeed = 0.125f;
        int animationFrame = 0;
        int animationFramesTotal = 4;

        public CoinPickup(Texture2D texture, Vector2 position, int value) : base(texture, position) {
            Texture = texture;
            Position = position;

            Value = value;
        }

        public void Update(GameTime gameTime) {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer < animationFrameSpeed) {
                animationTimer += elapsed;
                if (animationTimer >= animationFrameSpeed) {
                    animationFrame = (animationFrame + 1) % animationFramesTotal;
                    Texture = Graphics.PickupTexture_Coin[animationFrame];
                    animationTimer = 0f;
                }
            }

			if (Rect.X + Rect.Width < Game1.Camera.Rect.X && Active) {
				Active = false;
				Game1.PlayerStats.ResetMultiplier();
			}
        }

        public void Collect() {
			if (Active) {
				Game1.PlayerStats.TickScore();

				Active = false;
			}
        }
    }
}
