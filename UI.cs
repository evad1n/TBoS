using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    /// <summary>
    /// UI class draws UI (static) elements to the screen given a gamestate from Game1.
    /// 
    /// By Noah Bock and Dom Liotti
    /// </summary>
    /// 

    public enum MenuState { None, HighScore };
    public class UI {
        public PlayerStats PlayerStats;

        public Viewport viewport;

        MenuState MainMenuState = MenuState.None;

        float multiplierScaleFactor = 0.1f;
        float multiplierGrowTime = 0.15f;
        float growTimer;

        bool isGrowing;
        float currentScale;

        //Splash screen stuff
        float splashScreenDuration = 10f;
        float fadeSpeed = 0.5f;
        float ssTimer;

        float alphaValue = 255;
        float fadeIncrement;
        bool fading;

        public bool DoneWithSplashScreen = false;

		bool firstScoreDraw = true;
        
		private const float HighScorePopUpDelay = 2f;
		private float currentHSPopDelay = HighScorePopUpDelay;
		//private bool drawHSOverlay = false;

		//Button Stuff
		Button quitButton;
		Button restartButton;
		Button highscoreButton;
		Button playButton;
		Button helpButton;
		Button backButton;

		public UI(PlayerStats playerStats, Viewport viewport, Game1 game1) {
            PlayerStats = playerStats;
            this.viewport = viewport;

            fadeIncrement = -255 / fadeSpeed;

			quitButton = new Button(Graphics.MenuButtons[0], Graphics.MenuButtons[1], new Vector2(viewport.Width - ((2 + Graphics.MenuButtons[0].Width) * Game1.PIXEL_SCALE), 0 + (2 * Game1.PIXEL_SCALE)), game1.Exit);
			restartButton = new Button(Graphics.MenuButtons[9], Graphics.MenuButtons[10], new Vector2(2 * Game1.PIXEL_SCALE, viewport.Height - ((2 + Graphics.MenuButtons[9].Height)* Game1.PIXEL_SCALE)), game1.toPlayState);
			highscoreButton = new Button(Graphics.MenuButtons[2], Graphics.MenuButtons[3], new Vector2((viewport.Width / 2 - ((Graphics.MenuButtons[2].Width / 2) * Game1.PIXEL_SCALE) - ((2 + Graphics.MenuButtons[11].Width)) * Game1.PIXEL_SCALE), viewport.Height - ((2 + Graphics.MenuButtons[2].Height) * Game1.PIXEL_SCALE)), game1.toHSScreen);
			playButton = new Button(Graphics.MenuButtons[11], Graphics.MenuButtons[12], new Vector2(viewport.Width/2 - ((Graphics.MenuButtons[11].Width/2) * Game1.PIXEL_SCALE), viewport.Height - ((2 + Graphics.MenuButtons[11].Height) * Game1.PIXEL_SCALE)), game1.toPlayState);
			helpButton = new Button(Graphics.MenuButtons[4], Graphics.MenuButtons[5], new Vector2((viewport.Width / 2 - ((Graphics.MenuButtons[4].Width / 2) * Game1.PIXEL_SCALE) + ((2 + Graphics.MenuButtons[11].Width)) * Game1.PIXEL_SCALE), viewport.Height - ((2 + Graphics.MenuButtons[4].Height) * Game1.PIXEL_SCALE)), Ping);
			backButton = new Button(Graphics.MenuButtons[6], Graphics.MenuButtons[7], new Vector2(viewport.Width - ((2 + Graphics.MenuButtons[0].Width) * Game1.PIXEL_SCALE), 0 + (2 * Game1.PIXEL_SCALE)), game1.toMainMenu);
		}

        public void Update(GameTime gameTime, GameState state) {
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

			switch (state) {
				case GameState.SplashScreen:
					break;
				case GameState.MainMenu:
					highscoreButton.Update();
					playButton.Update();
					helpButton.Update();
					if (MainMenuState == MenuState.None)
						quitButton.Update();
					if (MainMenuState == MenuState.HighScore)
						backButton.Update();
					break;
				case GameState.Playing:
					break;
				case GameState.Pause:
					restartButton.Update();
					backButton.Update();
					break;
				case GameState.GameOver:
					restartButton.Update();
					backButton.Update();
					break;
			}
		}

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GameState gameState) {
            switch (gameState) {
                case GameState.SplashScreen:
                    DrawSplashScreen(spriteBatch, gameTime, Color.White);
                    break;

                case GameState.MainMenu:
                    switch (MainMenuState)
                    {
                        case MenuState.None:
                            spriteBatch.Draw(
                                Graphics.Title,
                                new Rectangle(
                                    viewport.Width / 2 - Graphics.Title.Width * Game1.PIXEL_SCALE / 2,
                                    3 * Game1.PIXEL_SCALE,
                                    Graphics.Title.Width * Game1.PIXEL_SCALE,
                                    Graphics.Title.Height * Game1.PIXEL_SCALE
                                ),
                                Color.White);

							quitButton.Draw(spriteBatch);
							highscoreButton.Draw(spriteBatch);
							playButton.Draw(spriteBatch);
							helpButton.Draw(spriteBatch);

							break;

                        case MenuState.HighScore:
							DrawHighScoreOverlay(spriteBatch);

							backButton.Draw(spriteBatch);

							break;
                    }
                    

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

					restartButton.Draw(spriteBatch);
					backButton.Draw(spriteBatch);

					break;

                case GameState.GameOver:

					var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
					currentHSPopDelay -= timer;
					if (currentHSPopDelay <= 0)
						DrawHighScoreOverlay(spriteBatch);
					else 
						spriteBatch.DrawString(
							Graphics.Font_Main,
							"Game Over!",
							new Vector2(viewport.Width / 2 - Graphics.Font_Main.MeasureString("Game Over").X * Game1.PIXEL_SCALE / 2, viewport.Height / 2 - Graphics.Font_Main.MeasureString("Game Over").Y * Game1.PIXEL_SCALE / 2),
							Color.White, 0, Vector2.Zero, Game1.PIXEL_SCALE, SpriteEffects.None, 1);

					restartButton.Draw(spriteBatch);
					backButton.Draw(spriteBatch);

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

		public void DrawHighScoreOverlay(SpriteBatch spriteBatch) {
			firstScoreDraw = true;
			spriteBatch.Draw(
				Graphics.HighScoreTextures[0],
				new Rectangle(
					viewport.Width / 2 - Graphics.HighScoreTextures[0].Width * Game1.PIXEL_SCALE / 2,
					3 * Game1.PIXEL_SCALE,
					Graphics.HighScoreTextures[0].Width * Game1.PIXEL_SCALE,
					Graphics.HighScoreTextures[0].Height * Game1.PIXEL_SCALE
					),
				Color.White);

			Rectangle firstRect = new Rectangle();

			for (int i = 0; i < Game1.Score.Score.Count; i++) {

				Rectangle rect = new Rectangle(
						viewport.Width / 2 - Graphics.HighScoreTextures[0].Width * Game1.PIXEL_SCALE / 2,
						(15 * Game1.PIXEL_SCALE + Graphics.HighScoreTextures[0].Height * Game1.PIXEL_SCALE) + (i * ((int)Graphics.Font_Main.MeasureString(" ").Y + 3) * Game1.PIXEL_SCALE),
						Graphics.HighScoreTextures[0].Width * Game1.PIXEL_SCALE,
						((int)Graphics.Font_Main.MeasureString(" ").Y + 1) * Game1.PIXEL_SCALE + Game1.PIXEL_SCALE
						);
				if (i == 0)
					firstRect = rect;

				//Draw a background and the highscore
				spriteBatch.Draw(
					Graphics.MenuBackground,
					rect,
					Color.White);

				string s = string.Format("{0:#,###0}", Game1.Score.Score[i]);
				Vector2 mSize = Graphics.Font_Main.MeasureString(s);
				Vector2 sSize = Graphics.Font_Small.MeasureString(s);

				if (s.Equals(string.Format("{0:#,###0}", Game1.Score.mostRecentScore)) && firstScoreDraw)
					if (i == 0) {
						spriteBatch.DrawString(Graphics.Font_Main, s, new Vector2(rect.X + rect.Width / 2 - mSize.X * Game1.PIXEL_SCALE / 2, rect.Y), new Color(255, 174, 12, 255), 0, Vector2.Zero, Game1.PIXEL_SCALE, SpriteEffects.None, 0);
						firstRect.X = (int)(rect.X + rect.Width / 2 - mSize.X * Game1.PIXEL_SCALE / 2);
						firstScoreDraw = false;
					}
					else {
						spriteBatch.DrawString(Graphics.Font_Small, s, new Vector2(rect.X + rect.Width / 2 - sSize.X * Game1.PIXEL_SCALE / 2, rect.Y + rect.Height / 2 - mSize.Y), new Color(255, 174, 12, 255), 0, Vector2.Zero, Game1.PIXEL_SCALE, SpriteEffects.None, 0);
						firstScoreDraw = false;
					}
				else {
					if (i == 0) {
						spriteBatch.DrawString(Graphics.Font_Main, s, new Vector2(rect.X + rect.Width / 2 - mSize.X * Game1.PIXEL_SCALE / 2, rect.Y), Color.White, 0, Vector2.Zero, Game1.PIXEL_SCALE, SpriteEffects.None, 0);
						firstRect.X = (int)(rect.X + rect.Width / 2 - mSize.X * Game1.PIXEL_SCALE / 2);
					}
					else
						spriteBatch.DrawString(Graphics.Font_Small, s, new Vector2(rect.X + rect.Width / 2 - sSize.X * Game1.PIXEL_SCALE / 2, rect.Y + rect.Height / 2 - mSize.Y), Color.White, 0, Vector2.Zero, Game1.PIXEL_SCALE, SpriteEffects.None, 0);
				}
			}

			spriteBatch.Draw(
				texture: Graphics.HighScoreTextures[1],
				destinationRectangle: new Rectangle(firstRect.X - Graphics.HighScoreTextures[1].Width - 3 * Game1.PIXEL_SCALE, firstRect.Y - ((Graphics.HighScoreTextures[1].Height - 4) * Game1.PIXEL_SCALE), Graphics.HighScoreTextures[1].Width * Game1.PIXEL_SCALE, Graphics.HighScoreTextures[1].Height * Game1.PIXEL_SCALE),
				color: Color.White,
				rotation: -0.2f);

		}

		public void ChangeMenuState(MenuState ms) {
			MainMenuState = ms;
		}


		// REMOVE THIS IN A SECOND
		private void Ping() {
			Console.WriteLine("Boop");
		}

		public void Reset() {
			currentHSPopDelay = HighScorePopUpDelay;
		}

	}
}
