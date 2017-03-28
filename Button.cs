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
        Texture2D texture;
        public Vector2 Position;

        public Rectangle Rect
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height); }
        }

        public Button(Texture2D texture, Vector2 position)
        {

        }
    }
}
