using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    public class UI {
        public PlayerStats PlayerStats;

        public Viewport viewport;

        public UI(PlayerStats playerStats, Viewport viewport) {
            PlayerStats = playerStats;
            this.viewport = viewport;
        }

        public void Draw(SpriteBatch spriteBatch, GameState gameState) {
            switch (gameState) {
                case GameState.Playing:
                    //Draw the player's score
                    spriteBatch.DrawString(
                        Graphics.Font_Main,
                        "Score " + PlayerStats.Score,
                        new Vector2(Game1.PIXEL_SCALE, (Graphics.UI_Hearts[0].Height + 1) * Game1.PIXEL_SCALE),
                        Color.White, 0, Vector2.Zero, Game1.PIXEL_SCALE, SpriteEffects.None, 1);

                    //Draw the player's time and distance
                    spriteBatch.DrawString(
                        Graphics.Font_Small,
                        "Time " + PlayerStats.Time.ToString("0.0") + " Dist " + PlayerStats.Distance.ToString("0.0"),
                        new Vector2(Game1.PIXEL_SCALE, (Graphics.UI_Hearts[0].Height + Graphics.Font_Main.LineSpacing + 2) * Game1.PIXEL_SCALE),
                        Color.White, 0, Vector2.Zero, Game1.PIXEL_SCALE, SpriteEffects.None, 1);


                    //Draw the player's Health
                    for (int i = 0; i < PlayerStats.MaxHealth / 2; i++) {

                        int halfHealth = PlayerStats.Health / 2;
                        int indexToDraw = 0;

                        if (i < halfHealth)
                            indexToDraw = 0;
                        else if (i == halfHealth) {
                            if (i * 2 + 1 == PlayerStats.Health)
                                indexToDraw = 1;
                            else
                                indexToDraw = 2;
                        } else
                            indexToDraw = 2;

                        spriteBatch.Draw(Graphics.UI_Hearts[indexToDraw], new Rectangle(Game1.PIXEL_SCALE + (i * (Graphics.UI_Hearts[0].Width + 1) * Game1.PIXEL_SCALE), Game1.PIXEL_SCALE, Graphics.UI_Hearts[0].Width * Game1.PIXEL_SCALE, Graphics.UI_Hearts[0].Height * Game1.PIXEL_SCALE), Color.White);
                    }
                    break;

                case GameState.GameOver:
                    spriteBatch.DrawString(
                        Graphics.Font_Main,
                        "Game Over!",
                        new Vector2(viewport.Width / 2 - Graphics.Font_Main.MeasureString("Game Over").X * Game1.PIXEL_SCALE / 2, viewport.Height / 2 - Graphics.Font_Main.MeasureString("Game Over").Y * Game1.PIXEL_SCALE / 2),
                        Color.White, 0, Vector2.Zero, Game1.PIXEL_SCALE, SpriteEffects.None, 1);
                    break;
            }
        }
    }
}
