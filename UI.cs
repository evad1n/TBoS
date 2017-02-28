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

		SpriteFont subtitleFontLarge;

		public void LoadContent(ContentManager Content) {
			subtitleFontLarge = Content.Load<SpriteFont>("graphics/ui/SubtitleFontLarge");
		}

		public void Draw(SpriteBatch sb) {
			sb.DrawString(subtitleFontLarge, "BLAH BLAH blah blah", new Vector2(20, 20),Color.WhiteSmoke);
		}
	}
}
