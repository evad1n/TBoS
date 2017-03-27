using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    public class UI {
        public PlayerStats PlayerStats;

        public Viewport viewport;

        float multiplierScaleFactor = 0.1f;
        float multiplierGrowTime = 0.15f;
        float growTimer;

        bool isGrowing;
        float currentScale;

        string instructions = 
            "                              Press Enter to Start the game.\n\n" +
            "                 Use A and D or the left and right arrow keys to move.\n" + 
            "              Use the W or Up arrow key to jump. Hold it to jump higher.\n" +
            "           When touching a wall, press W or the Up arrow key to jump again.\n\n" + 
            "                     Increase your score multiplier by collecting coins.\n" + 
            "                      If you miss a coin, your multiplier returns to 1.\n\n" + 
            "Press Escape in-game to pause. While paused, press Enter to return to this menu.\n" +
            "                     Press Escape to restart the game when you die.";


        //Splash screen stuff
        float splashScreenDuration = 10f;
        float fadeSpeed = 0.5f;
        float ssTimer;

        float alphaValue = 255;
        float fadeIncrement;
        bool fading;

        public bool DoneWithSplashScreen = false;

        public UI(PlayerStats playerStats, Viewport viewport) {
            PlayerStats = playerStats;
            this.viewport = viewport;

            fadeIncrement = -255 / fadeSpeed;
        }

        public void Update(GameTime gameTime) {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (ssTimer < splashScreenDuration) {

                ssTimer += elapsed;

                if (ssTimer >= splashScreenDuration)
                    DoneWithSplashScreen = true;

                if (ssTimer >= 0f && ssTimer < splashScreenDuration * 0.05f)
                    fading = true;
                else if (ssTimer >= splashScreenDuration * .05f && ssTimer < splashScreenDuration * .45f)
                    fading = false;
                else if (ssTimer >= splashScreenDuration * .45f && ssTimer < splashScreenDuration * .55f)
                    fading = true;
                else if (ssTimer >= splashScreenDuration * .55f && ssTimer < splashScreenDuration * .95f)
                    fading = false;
                else if (ssTimer >= splashScreenDuration * .95f && ssTimer < splashScreenDuration * 1.0)
                    fading = true;
            }

            if (fading) {
                alphaValue += elapsed * fadeIncrement;

                if (alphaValue >= 255 || alphaValue <= 0)
                    fadeIncrement *= -1;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GameState gameState) {
            switch (gameState) {
                case GameState.SplashScreen:
                    DrawSplashScreen(spriteBatch, gameTime, Color.White);
                    break;

                case GameState.MainMenu:
                    spriteBatch.Draw(
                        Graphics.Title,
                        new Rectangle(
                            viewport.Width / 2 - Graphics.Title.Width * Game1.PIXEL_SCALE / 2,
                            3 * Game1.PIXEL_SCALE,
                            Graphics.Title.Width * Game1.PIXEL_SCALE,
                            Graphics.Title.Height * Game1.PIXEL_SCALE
                            ), 
                        Color.White);

                    spriteBatch.DrawString(Graphics.Font_Outlined,
                        instructions,
                        new Vector2(viewport.Width / 2 - Graphics.Font_Outlined.MeasureString(instructions).X, viewport.Height / 2),
                        Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);

                    break;

                case GameState.Playing:

					//Draw the player's score
					DrawScore(spriteBatch);

					//Draw the player's time and distance
					//DrawTechnicalScores(spriteBatch);

					//Draw the player's Health
					DrawHealth(spriteBatch);

					DrawMultiplier(spriteBatch, gameTime);

					break;

                case GameState.Pause:
                    spriteBatch.Draw(Graphics.Overlay, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);

                    //Draw the player's score
                    DrawScore(spriteBatch);

					//Draw the player's time and distance
					DrawTechnicalScores(spriteBatch);

					//Draw the player's Health
					DrawHealth(spriteBatch);

					DrawMultiplier(spriteBatch, gameTime);

                    spriteBatch.DrawString(
                        Graphics.Font_Main,
                        "PAUSED",
                        new Vector2(viewport.Width / 2 - Graphics.Font_Main.MeasureString("Game Over").X * Game1.PIXEL_SCALE / 2, viewport.Height / 2 - Graphics.Font_Main.MeasureString("Game Over").Y * Game1.PIXEL_SCALE / 2),
                        Color.White, 0, Vector2.Zero, Game1.PIXEL_SCALE, SpriteEffects.None, 1);

                    break;

                case GameState.GameOver:
					//Draw the player's score
					DrawScore(spriteBatch);

					//Draw the player's time and distance
					//DrawTechnicalScores(spriteBatch);

					//Draw the player's Health
					DrawHealth(spriteBatch);

					DrawMultiplier(spriteBatch, gameTime);
                    
					spriteBatch.DrawString(
                        Graphics.Font_Main,
                        "Game Over!",
                        new Vector2(viewport.Width / 2 - Graphics.Font_Main.MeasureString("Game Over").X * Game1.PIXEL_SCALE / 2, viewport.Height / 2 - Graphics.Font_Main.MeasureString("Game Over").Y * Game1.PIXEL_SCALE / 2),
                        Color.White, 0, Vector2.Zero, Game1.PIXEL_SCALE, SpriteEffects.None, 1);

                    break;
            }
        }

		private void DrawHealth(SpriteBatch spriteBatch) {
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
				}
				else
					indexToDraw = 2;

				spriteBatch.Draw(
                    Graphics.UI_Hearts[indexToDraw], 
                    new Rectangle(
                        Game1.PIXEL_SCALE * 5 + (i * (Graphics.UI_Hearts[0].Width + 1) * Game1.PIXEL_SCALE), 
                        Game1.PIXEL_SCALE * 5, 
                        Graphics.UI_Hearts[0].Width * Game1.PIXEL_SCALE, 
                        Graphics.UI_Hearts[0].Height * Game1.PIXEL_SCALE), 
                    PlayerStats.invulnColor);
			}
		}

		private void DrawTechnicalScores(SpriteBatch spriteBatch) {
            string s = "Time " + PlayerStats.Time.ToString("0.0") + " Dist " + PlayerStats.Distance.ToString("0.0");


            spriteBatch.DrawString(
            Graphics.Font_Small,
            s,
            new Vector2(viewport.Width / 2 - Graphics.Font_Small.MeasureString(s).X * Game1.PIXEL_SCALE / 2, viewport.Height - (Graphics.Font_Small.MeasureString(s).Y / 2 + 20)),
			Color.White, 0, Vector2.Zero, Game1.PIXEL_SCALE, SpriteEffects.None, 1);
		}

		private void DrawScore(SpriteBatch spriteBatch) {
            spriteBatch.Draw(
                Graphics.Icons[1],
                new Rectangle(
                    Game1.PIXEL_SCALE * 5,
                    (Graphics.UI_Hearts[0].Height + 8) * Game1.PIXEL_SCALE,
                    Graphics.Icons[1].Width * Game1.PIXEL_SCALE,
                    Graphics.Icons[1].Height * Game1.PIXEL_SCALE),
                Color.White);

            spriteBatch.DrawString(
                Graphics.Font_Main,
                string.Format("{0:#,###0}", Game1.PlayerStats.Score), 
                new Vector2(
                    Game1.PIXEL_SCALE * 6 + Graphics.Icons[1].Width * Game1.PIXEL_SCALE,
                    (Graphics.UI_Hearts[0].Height + 6) * Game1.PIXEL_SCALE), 
                Color.White, 0, Vector2.Zero, Game1.PIXEL_SCALE, SpriteEffects.None, 0);
		}

		private void DrawMultiplier(SpriteBatch spriteBatch, GameTime gameTime) {
            float scale = PlayerStats.ScoreMultiplier * 1.1f;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(growTimer < multiplierGrowTime) {
                growTimer += elapsed;

                if(growTimer >= multiplierGrowTime) {
                    growTimer = 0f;
                    isGrowing = !isGrowing;
                }
            }

            if (isGrowing)
                currentScale = MathHelper.Lerp(currentScale, 1 + PlayerStats.ScoreMultiplier * multiplierScaleFactor, multiplierGrowTime);
            else
                currentScale = MathHelper.Lerp(currentScale, 1, multiplierGrowTime);


            spriteBatch.Draw(
                Graphics.UI_Multipliers[PlayerStats.ScoreMultiplier - 1], 
                destinationRectangle: new Rectangle(
                    (20 + (int)(Math.Floor(Math.Log10(PlayerStats.Score)) + 1) * 7) * Game1.PIXEL_SCALE,
                    (8 + Graphics.UI_Hearts[0].Height) * Game1.PIXEL_SCALE, 
				    (int)(Graphics.UI_Multipliers[PlayerStats.ScoreMultiplier - 1].Width * Game1.PIXEL_SCALE * currentScale), 
				    (int)(Graphics.UI_Multipliers[PlayerStats.ScoreMultiplier - 1].Height * Game1.PIXEL_SCALE * currentScale)), 
				color: Color.White,
				rotation: -0.3f);

			DrawMultiplierTicks(spriteBatch);
		}

        private void DrawSplashScreen(SpriteBatch spriteBatch, GameTime gameTime, Color white)
        {

            if (ssTimer < splashScreenDuration / 2)
            { //draw powered by monogame
                spriteBatch.DrawString(
                    Graphics.Font_Small,
                    "Powered By",
                    new Vector2(
                        viewport.Width / 2 - Graphics.Font_Small.MeasureString("Powered By").X * 2 / 2,
                        viewport.Height / 2 - (Graphics.SplashScreenGraphics[0].Height * 2) / 2 - Graphics.SplashScreenGraphics[0].Height * 2),
                    Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);

                spriteBatch.Draw(
                    Graphics.SplashScreenGraphics[0],
                    new Rectangle(
                        viewport.Width / 2 - (Graphics.SplashScreenGraphics[0].Width * 2) / 2,
                        viewport.Height / 2 - (Graphics.SplashScreenGraphics[0].Height * 2) / 2,
                        Graphics.SplashScreenGraphics[0].Width * 2,
                        Graphics.SplashScreenGraphics[0].Height * 2),
                    Color.White);
            }
            else
            { //draw names/logo
                spriteBatch.Draw(
                    Graphics.Logo,
                    new Rectangle(
                        viewport.Width / 2 - (Graphics.Logo.Width * 2) / 2,
                        viewport.Height / 2 - (Graphics.Logo.Height * 2) / 2,
                        Graphics.Logo.Width * 2,
                        Graphics.Logo.Height * 2),
                    Color.White);

                string credits = "";

                for (int i = 0; i < Game1.DEVELOPER_NAMES.Length; i++) {
                    if(i == Game1.DEVELOPER_NAMES.Length - 1)
                        credits += Game1.DEVELOPER_NAMES[i];
                    else
                        credits += Game1.DEVELOPER_NAMES[i] + " ~ ";
                }

                spriteBatch.DrawString(
                    Graphics.Font_Small, 
                    credits,
                    new Vector2(
                        viewport.Width / 2 - Graphics.Font_Small.MeasureString(credits).X * 2 / 2,
                        viewport.Height - (Graphics.Font_Small.MeasureString(credits).Y * 2 + 20)),
                    Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            }

            //Draw the fading texture
            spriteBatch.Draw(Graphics.BlackTexture, new Rectangle(0, 0, viewport.Width, viewport.Height), new Color(0, 0, 0, (int)MathHelper.Clamp(alphaValue, 0, 255)));
        }

        bool IsButtonPressed(Rectangle button, MouseState state)
        {
            return state.LeftButton == ButtonState.Pressed && state.X >= button.X && state.X <= button.Right && state.Y >= button.Y && state.Y <= button.Bottom;
        }

		private void DrawMultiplierTicks(SpriteBatch spriteBatch) {

			for (int i = 0; i < 4; i++) {
                Texture2D toDraw = null;

                if (i < Game1.PlayerStats.ScoreMultiTicks)
                    toDraw = Graphics.UI_MultiplierIndicators[0];
				else
                    toDraw = Graphics.UI_MultiplierIndicators[1];

                spriteBatch.Draw(
                    toDraw,
                    new Rectangle(
                        i * ((1 + Graphics.UI_MultiplierIndicators[0].Width) * Game1.PIXEL_SCALE) + ((5 + Graphics.UI_Hearts[0].Width/2 - toDraw.Width/2) * Game1.PIXEL_SCALE),
                        (10 + Graphics.UI_Hearts[0].Height + Graphics.Font_Main.LineSpacing) * Game1.PIXEL_SCALE,
                        Graphics.UI_MultiplierIndicators[0].Width * Game1.PIXEL_SCALE,
                        Graphics.UI_MultiplierIndicators[0].Height * Game1.PIXEL_SCALE),
                    Color.White);
            }
		}
	}
}
