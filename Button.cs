using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    public class Button
    {
        public Texture2D Texture;
		Texture2D nonClickedTexture;
		Texture2D clickedTexture;
        public Vector2 Position;

		//public bool clicked;

		Action method;

        public Rectangle Rect
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height); }
        }

        public Button(Texture2D nonClickedTexture, Texture2D clickedTexture, Vector2 position, Action method)
        {
			this.nonClickedTexture = nonClickedTexture;
			this.clickedTexture = clickedTexture;
			Texture = nonClickedTexture;
			Position = position;

			//clicked = false;

			this.method = method;
        }

		public void Update() {
			if (Game1.mouseState.LeftButton == ButtonState.Released && Game1.prevMouseState.LeftButton == ButtonState.Pressed && CheckMouseLocation()) {
				Click();
			}
		}

		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(nonClickedTexture, Rect, Color.White);
		}


		private void Click() {
			method();
		}

		private bool CheckMouseLocation() {
			if (!Rect.Contains(Game1.mouseState.X, Game1.mouseState.Y))
				return false;
			if (!Rect.Contains(Game1.prevMouseState.X, Game1.prevMouseState.Y))
				return false;
			return true;
		}
    }
}
