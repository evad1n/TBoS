using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    public class TileDecoration {
        private Texture2D texture;
        public Texture2D Texture {
            get { return texture; }
            set { texture = value; }
        }

        private Rectangle rect;
        public Rectangle Rect {
            get { return rect; }
            protected set { rect = value; }
        }

        public TileDecoration(Rectangle r, int id = 0) {
            Rect = r;
            Texture = PickTexture(id, Game1.RandomObject);
        }

        //Pretty self explanatory
        Texture2D PickTexture(int ID, Random r) {
            switch (ID) {
                case 0:
                    return Graphics.Deco_groundTop[r.Next(0, Graphics.Deco_groundTop.Length)];
                case 1:
                    return Graphics.Deco_groundBottom[r.Next(0, Graphics.Deco_groundBottom.Length)];
                case 2:
                    return Graphics.Deco_backgroundMiddle[r.Next(0, Graphics.Deco_backgroundMiddle.Length)];
                case 3:
                    return Graphics.Deco_backgroundBottom[r.Next(0, Graphics.Deco_backgroundBottom.Length)];
                case 4:
                    return Graphics.Deco_groundMiddle[r.Next(0, Graphics.Deco_groundMiddle.Length)];
                case 5:
                    return Graphics.Deco_stoneMiddle[r.Next(0, Graphics.Deco_stoneMiddle.Length)];
                default:
                    return Graphics.Deco_groundTop[r.Next(0, Graphics.Deco_groundTop.Length)];
            }
        }

        public void Draw(SpriteBatch sb, Color color) {
            sb.Draw(Texture, new Rectangle(Rect.X - Rect.Size.X / 2, Rect.Y - Rect.Size.Y / 2, Rect.Width, Rect.Height), color);
        }
    }
}
