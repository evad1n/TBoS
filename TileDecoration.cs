using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBondOfStone {
    class TileDecoration {
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

        private static ContentManager content;
        public static ContentManager Content {
            protected get { return content; }
            set { content = value; }
        }

        public TileDecoration(Rectangle r, int id = 0) {
            Rect = r;
            Content = content;
            Texture = PickTexture(id, Game1.RandomObject);
        }

        Texture2D PickTexture(int ID, Random r) {
            if (ID == 0)
                return Content.Load<Texture2D>(@"graphics\deco\deco_0_" + r.Next(1, 12));
            if(ID == 1)
                return Content.Load<Texture2D>(@"graphics\deco\deco_1_" + r.Next(1, 3));
            if (ID == 2)
                return Content.Load<Texture2D>(@"graphics\deco\deco_2_" + r.Next(1, 3));
            if (ID == 3)
                return Content.Load<Texture2D>(@"graphics\deco\deco_3_" + r.Next(1, 3));
            
            return Content.Load<Texture2D>(@"graphics\deco\deco_0_" + r.Next(1, 12));
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(Texture, Rect, Color.White);
        }
    }
}
