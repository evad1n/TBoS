using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    public class TitanManager {

        public bool HasTitan { get { return activeTitan != null; } }

        Titan activeTitan = null;

        Viewport viewport;

        float titanTimer = 0;
        float titanSpawnTimerMax = 10f;
        int titanSpawnRate;

        int numTitans = 1;

        public TitanManager(Viewport viewport) {
            this.viewport = viewport;

            titanSpawnRate = Game1.TITAN_SPAWN_RATE;
        }

        public void Update(GameTime gameTime) {
            //Do titan spawning
            if (activeTitan == null) {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;


                if (titanTimer < titanSpawnTimerMax) {
                    titanTimer += elapsed;

                    if (titanTimer >= titanSpawnTimerMax) {
                        if (Game1.RandomObject.Next(0, titanSpawnRate + 1) == 0) {
                            //Spawn a titan
                            switch (Game1.RandomObject.Next(0, numTitans)) {
                                //Spawn Tyche, the Sundered
                                case 0:
                                    activeTitan = new Titan(Graphics.Titan_Tyche[0], new Vector2(viewport.Bounds.Left - Graphics.Titan_Tyche[0].Width * Game1.PIXEL_SCALE, 20), 0.125f, Graphics.Titan_Tyche, true);
                                    break;

                                //Spawn Nemesis, Annihilator
                                case 1:
                                    activeTitan = new Titan(Graphics.Titan_Nemesis[0], new Vector2(viewport.Bounds.Right, 20), -1f, Graphics.Titan_Nemesis, false);
                                    break;
                            }
                        }

                        titanTimer = 0f;
                    }
                }
            } else {
                activeTitan.Update(gameTime);

                if (activeTitan.FacingRight && activeTitan.Rect.X > viewport.Bounds.Right)
                    activeTitan = null;
                else if (!activeTitan.FacingRight && activeTitan.Rect.Right > viewport.Bounds.Left)
                    activeTitan = null;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (activeTitan != null) {
                activeTitan.Draw(spriteBatch, Color.White, 0);
            }
        }

        public void Reset() {
            activeTitan = null;
            titanTimer = 0f;
        }
    }
}
