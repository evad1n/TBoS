using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TheBondOfStone {
	class UI {

		Game1 game;
		PlayerStats playStats;

		SpriteFont subtitleFontLarge;

		// HUD elements
		Texture2D heartSheet;
		Texture2D scoreBitSheet;
		Texture2D multiplierSheet;

		// HUD rectangles
		Rectangle[] heartFrames;
		Rectangle[] scorebitFrames;
		Rectangle[] multiplierFrames;

		public UI(Game1 game, PlayerStats playStats) {
			this.game = game;
			this.playStats = playStats;
		}

		public void LoadContent(ContentManager Content) {
			// Fonts
			subtitleFontLarge = Content.Load<SpriteFont>("graphics/ui/SubtitleFontLarge");

			// HUD elements
			heartSheet = Content.Load<Texture2D>("graphics/ui/HeartsSpriteSheet");
			scoreBitSheet = Content.Load<Texture2D>("graphics/ui/ScoreBitsSpriteSheet");
			multiplierSheet = Content.Load<Texture2D>("graphics/ui/ScoreMultiplierSpriteSheet");

			heartFrames = SpriteSheetManager.Split(9, 8, 3);
			scorebitFrames = SpriteSheetManager.Split(6, 6, 2);
			multiplierFrames = SpriteSheetManager.Split(21, 16, 4);
		}

		public void Draw(SpriteBatch sb) {

			if (game.state == GameState.Playing) {
				sb.DrawString(subtitleFontLarge, "BLAH BLAH blah blah", new Vector2(20, 20), Color.WhiteSmoke);

				for (int i = 0; i < playStats.Health / 2; i++) {
					sb.Draw(heartSheet, new Rectangle(10 + 11 * i* Game1.UIScaleFactor, 25 * Game1.UIScaleFactor, 9 * Game1.UIScaleFactor, 8 * Game1.UIScaleFactor), heartFrames[0], Color.White);
				}
				if (playStats.Health % 2 == 1)
					sb.Draw(heartSheet, new Rectangle(10 + 11 * (playStats.Health / 2 % 3) * Game1.UIScaleFactor, 25 * Game1.UIScaleFactor, 9 * Game1.UIScaleFactor, 8 * Game1.UIScaleFactor), Color.White);
				if (playStats.Health < 5)
					sb.Draw(heartSheet, new Rectangle(32 * Game1.UIScaleFactor, 25 * Game1.UIScaleFactor, 9 * Game1.UIScaleFactor, 8 * Game1.UIScaleFactor), heartFrames[2], Color.White);
				if (playStats.Health < 3)
					sb.Draw(heartSheet, new Rectangle(21 * Game1.UIScaleFactor, 25 * Game1.UIScaleFactor, 9 * Game1.UIScaleFactor, 8 * Game1.UIScaleFactor), heartFrames[2], Color.White);
			}
		}
	}
}
