
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBondOfStone {

    /// <summary>
    /// Controls the collision detection and response behavior of a tile.
    /// </summary>
    enum TileCollision {
        /// <summary>
        /// A passable tile is one which does not hinder player motion at all.
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// A platform tile is one which behaves like a passable tile except when the
        /// player is above it. A player can jump up through a platform as well as move
        /// past it to the left and right, but can not fall down through the top of it.
        /// </summary>
        Platform = 2,
    }

    class Tile {
        public int DrawQueue { get; set; }

        public bool IsEndTile { get; set; }
        public bool IsStartTile { get; set; }

        public TileCollision Collision;

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

        List<TileDecoration> decorations = new List<TileDecoration>();
        public List<TileDecoration> Decorations {
            get { return decorations; }
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

        public Tile(int ID, Rectangle r, TileCollision collision) {

            this.ID = ID;
            Rect = r; 
            Collision = collision;

            if (ID == 1 || ID == 3 || ID == 4 || ID == 5) { //Draw queue means background tiles are rendered behind ground tiles
                DrawQueue = 0;
                texture = Game1.emptyTile;

            } else {
                DrawQueue = -1;
                texture = Game1.emptyTile;
            }

        }

        public Tile(Texture2D texture, Rectangle rect) {
            Texture = texture;
            Rect = rect;

            DrawQueue = -2;
            stitched = true;
        }

        public void Draw(SpriteBatch sb, Color color) {
            if(!stitched)
                StitchTile();

                sb.Draw(texture, Rect, null, color, 0, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);

            foreach (TileDecoration d in Decorations)
                d.Draw(sb, color);
        }

        //Use bitmasking to determine which texture this tile should have
        public void StitchTile() {
            //Calculate the bitmask value
            //North = 2^0 = 1, West = 2^1 = 2, East = 2^2 = 4, South = 2^3 = 8
            if (ID == 4)
                adjacents[1] = true;
            if (ID == 5)
                adjacents[2] = true;

            if (ID != 0) {
                int bmv = Convert.ToInt32(adjacents[0]) +
                    2 * Convert.ToInt32(adjacents[1]) +
                    4 * Convert.ToInt32(adjacents[2]) +
                    8 * Convert.ToInt32(adjacents[3]);

                if (ID == 4 || ID == 5) //Start and end tiles should be textured as Grass
                    Texture = Game1.foregroundTiles[bmv];
                else //Other tiles are textured according to their IDs
                {
                    if(ID == 1)
                    {
                        Texture = Game1.foregroundTiles[bmv];
                    }
                    if(ID == 2)
                    {
                        Texture = Game1.backgroundTiles[bmv];
                    }
                }
            }

            stitched = true;
        }

        public Tile AddBackgroundTile() {
            return new Tile(Game1.backgroundTiles[15], Rect);
        }

        public void GenerateDecorations() {
            switch (ID) {
                case 1:
                    if (Game1.RandomObject.Next(10) <= 3)
                        if(Adjacents[0] == false)
                            Decorations.Add(new TileDecoration(new Rectangle(Rect.X, Rect.Y - (Rect.Height - Rect.Height / 8), Rect.Width, Rect.Height), 0));

                    if (Adjacents[3] == false)
                        if (Game1.RandomObject.Next(10) <= 1) {
                                Decorations.Add(new TileDecoration(new Rectangle(Rect.X, Rect.Y + (Rect.Height - Rect.Height / 8), Rect.Width, Rect.Height), 1));
                        }
                break;

                case 2:
                    if (Game1.RandomObject.Next(10) == 0)
                        Decorations.Add(new TileDecoration(new Rectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height), 2));

                    if (Adjacents[3] == false)
                        if (Game1.RandomObject.Next(10) <= 1)
                            Decorations.Add(new TileDecoration(new Rectangle(Rect.X, Rect.Y + (Rect.Height - Rect.Height / 8), Rect.Width, Rect.Height), 3));
                    break;
            }
        }
    }
}
