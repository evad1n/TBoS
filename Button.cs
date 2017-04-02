using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
		public bool clicked;

        public Rectangle Rect
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height); }
        }

        public Button(Texture2D clickedTexture, Texture2D nonClickedTexture, Vector2 position)
        {
			this.nonClickedTexture = nonClickedTexture;
			this.clickedTexture = clickedTexture;
			Texture = nonClickedTexture;
			Position = position;

			clicked = false;
        }
    }
}
