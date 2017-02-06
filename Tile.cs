using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBondOfStone {
    class Tile {
        public int DrawQueue { get; set; }

        public bool IsEndTile { get; set; }
        public bool IsStartTile { get; set; }

        private Texture2D texture;
        public Texture2D Texture {
            get { return texture; }
            set { texture = value; }
        }

        private bool[] adjacents = new bool[4];
        public bool[] Adjacents {
            get { return adjacents; }
            set { adjacents = value; }
        }

        private Rectangle rect;
        public Rectangle Rect {
            get { return rect; }
            protected set { rect = value; }
        }
        
        bool stitched = false;

        private static ContentManager content;
        public static ContentManager Content {
            protected get { return content; }
            set { content = value; }
        }

        private int id;
        public int ID {
            get { return id; }
            set { id = value; }
        }

        public Tile(int ID, Rectangle r) {
            texture = Content.Load<Texture2D>("tile_" + ID);
            this.ID = ID;
            Rect = r;

            if (ID == 1)
                DrawQueue = 0;
            else
                DrawQueue = -1;
        }

        public Tile(Texture2D texture, Rectangle rect) {
            Texture = texture;
            Rect = rect;

            DrawQueue = -2;
            stitched = true;
        }

        public void Draw(SpriteBatch sb) {
            if(!stitched)
                StitchTile();
                
            sb.Draw(texture, rect, Color.White);
        }

        //Use bitmasking to determine which texture this tile should have
        public void StitchTile() {
            //Calculate the bitmask value
            //North = 2^0 = 1, West = 2^1 = 2, East = 2^2 = 4, South = 2^3 = 8
            if (ID != 0) {
                int bmv = Convert.ToInt32(adjacents[0]) +
                    2 * Convert.ToInt32(adjacents[1]) +
                    4 * Convert.ToInt32(adjacents[2]) +
                    8 * Convert.ToInt32(adjacents[3]);
                
                if(ID == 4 || ID == 5) //Start and end tiles should be textured as Grass
                    Texture = Content.Load<Texture2D>("graphics\\tile\\tile_1_" + bmv);
                else //Other tiles are textured according to their IDs
                    Texture = Content.Load<Texture2D>("graphics\\tile\\tile_" + ID + "_" + bmv);
            }

            stitched = true;
        }

        public Tile AddBackgroundTile() {
            return new Tile(Content.Load<Texture2D>("graphics\\tile\\tile_2_15"), Rect);
        }
    }
}
