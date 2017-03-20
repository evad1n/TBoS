using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    public class Tile {
        //The order of precedence of this tile when drawing.
        public int DrawQueue { get; set; }

        //Flags tell whether this is a start or end tile.
        public bool IsEndTile { get; set; }
        public bool IsStartTile { get; set; }

        //The texture this tile is represented with when drawing.
        private Texture2D texture;
        public Texture2D Texture {
            get { return texture; }
            set { texture = value; }
        }

        //Array holding four booleans, which define whether this tile has neighboring tiles
        //that it should stitch together with in its four cardinal directions, in the order
        //North, West, East, South.
        private bool[] adjacents = new bool[4];
        public bool[] Adjacents {
            get { return adjacents; }
            set { adjacents = value; }
        }

        //The decorations this tile has drawn on/above/below it.
        List<TileDecoration> decorations = new List<TileDecoration>();
        public List<TileDecoration> Decorations {
            get { return decorations; }
        }

        //The rect position and size of this tile.
        private Rectangle rect;
        public Rectangle Rect {
            get { return rect; }
            protected set { rect = value; }
        }

        //Has this tile undergone teh stitching process?
        bool stitched = false;

        //The ID of this tile.
        private int id;
        public int ID {
            get { return id; }
            set { id = value; }
        }

        public Tile(int ID, Rectangle r, Texture2D texture = null, bool stitched = false) {

            this.ID = ID;
            Rect = r;

            this.stitched = stitched;

            //Draw Queueing: draw tiles that should be in the background before anything else.
            if (ID != 2)
                DrawQueue = 1;
            else
                DrawQueue = 0;

            //Initialize the texture to an empty image if one isn't given.
            if (texture == null)
                this.texture = Graphics.EmptyTexture;
            else
                this.texture = texture;
        }

        public void Draw(SpriteBatch sb, Color color) {
            //Assign this tile an actual texture if it doesn't have one.
            if (!stitched)
                StitchTile();
            
            //Draw the tile.
            sb.Draw(texture, Rect, null, color, 0, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, DrawQueue);

            //Draw the tile's decorations.
            foreach (TileDecoration d in Decorations) {
                d.Draw(sb, Color.White);
            }
        }

        /// <summary>
        /// Use bitmasking and adjacency array to determine which texture this tile should have.
        /// </summary>
        public void StitchTile() {
            //Set the start and end tiles' adjacents so they stitch properly
            if (ID == 4)
                adjacents[1] = true;
            if (ID == 5)
                adjacents[2] = true;

            //Calculate the bitmask value
            //North = 2^0 = 1, West = 2^1 = 2, East = 2^2 = 4, South = 2^3 = 8
            if (ID != 0) {
                int bmv = Convert.ToInt32(adjacents[0]) +
                    2 * Convert.ToInt32(adjacents[1]) +
                    4 * Convert.ToInt32(adjacents[2]) +
                    8 * Convert.ToInt32(adjacents[3]);

                //Assign texture based on the calculated BitMask Value
                switch (ID) {
                    case 1:
                    case 4:
                    case 5:
                        Texture = Graphics.Tiles_ground[bmv];
                        break;
                    case 2:
                        Texture = Graphics.Tiles_background[bmv];
                        break;
                    case 3:
                        Texture = Graphics.Tiles_gold[bmv];
                        break;

                    default:
                        Texture = Graphics.EmptyTexture;
                        break;
                }
            }

            //The tile has been stitched.
            stitched = true;
        }

        /// <summary>
        /// Add a graphical tile to fix holes in the background layer.
        /// </summary>
        public Tile AddBackgroundTile() {
            return new Tile(2, Rect, Graphics.Tiles_background[15], true);
        }

        /// <summary>
        /// Randomly generates tile decorations.
        /// </summary>
        public void GenerateDecorations() {
            //Determine whether a decoration spawns
            switch (ID) {
                //Ground tiles spawn grasses, rocks, and vines where applicable.
                case 1:
                case 3:
                case 4:
                case 5:
                    //If this tile has nothing above it, have a chance to spawn a grass or rock.
                    if (Adjacents[0] == false)
                        if (Game1.RandomObject.Next(10) <= 3)
                            Decorations.Add(new TileDecoration(new Rectangle(Rect.X, Rect.Y - (Rect.Height - Rect.Height / Game1.TILE_PIXEL_SIZE), Rect.Width, Rect.Height), 0));

                    //If this tile has nothing under it, have a chance to spawn a vine.
                    if (Adjacents[3] == false)
                        if (Game1.RandomObject.Next(10) <= 1) {
                            Decorations.Add(new TileDecoration(new Rectangle(Rect.X, Rect.Y + (Rect.Height - Rect.Height / Game1.TILE_PIXEL_SIZE), Rect.Width, Rect.Height), 1));
                        }

                    //If this tile has something above, below, and to either side, have a chance to spawn a different stone texture.
                    if(Adjacents[0] && Adjacents[1] && Adjacents[2] && Adjacents[3] && ID != 3)
                        if (Game1.RandomObject.Next(10) <= 1) {
                            Decorations.Add(new TileDecoration(new Rectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height), 4));
                        }

                    if (ID == 3)
                            if (Game1.RandomObject.Next(10) == 0) {
                                Decorations.Add(new TileDecoration(new Rectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height), 5));
                            }
                    break;

                //Background tiles spawn rocky bits and rough bottoms where applicable.
                case 2:
                    //Have a chance to spawn middle roughness.
                    if (Game1.RandomObject.Next(10) == 0)
                        Decorations.Add(new TileDecoration(Rect, 2));

                    //If there's nothing below this tile, have a chance to spawn bottom roughness.
                    if (Adjacents[3] == false)
                        if (Game1.RandomObject.Next(10) <= 1)
                            Decorations.Add(new TileDecoration(new Rectangle(Rect.X, Rect.Y + (Rect.Height - Rect.Height / Game1.TILE_PIXEL_SIZE), Rect.Width, Rect.Height), 3));
                    break;
            }
        }
    }
}
