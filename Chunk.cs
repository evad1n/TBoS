using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBondOfStone {
    /// <summary>
    /// Represents a chunk's tile data.
    /// </summary>
    class Chunk {

        List<Tile> tiles = new List<Tile>(); //List holds tiles linearly (w/ property)

        public Tile StartTile { get; set; }
        public Tile EndTile { get; set; }

        public bool Generated { get; set; }

        /// <summary>
        /// The list of tiles in this chunk, in linear order.
        /// </summary>
        public List<Tile> Tiles {
            get { return tiles; }
        }

        public TileCollision[,] Collisions { get; set; }

        //Width and height of this chunk
        private int width, height;
        /// <summary>
        /// The width of the chunk.
        /// </summary>
        public int Width { get { return width; } }
        /// <summary>
        /// The height of the chunk.
        /// </summary>
        public int Height { get { return height; } }

        Microsoft.Xna.Framework.Rectangle o;

        public Chunk(Microsoft.Xna.Framework.Rectangle originTile, World world) {
            o = originTile;
        }

        //default constructor generates the "starter chunk"
        public Chunk() {

            int[,] atlas = new int[,] {
                { 0, 0, 0, 0, 0 },
                { 0, 2, 2, 0, 0 },
                { 0, 2, 2, 2, 0 },
                { 1, 1, 1, 1, 5 },
                { 0, 1, 1, 0, 0 }
            };

            Collisions = new TileCollision[,] {
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { TileCollision.Impassable, TileCollision.Impassable, TileCollision.Impassable, TileCollision.Impassable, TileCollision.Impassable },
                { 0, TileCollision.Impassable, TileCollision.Impassable, 0, 0 }
            };

            Generate(atlas, Game1.PixelScaleFactor);
            EndTile = Tiles[23];

            o = new Microsoft.Xna.Framework.Rectangle(10 * Game1.PixelScaleFactor, 20 * Game1.PixelScaleFactor, Game1.PixelScaleFactor, Game1.PixelScaleFactor);
        }

        //Generate this chunk in a drawable format. Takes a 2D array of tile IDs and a tile size.
        /// <summary>
        /// Generates the chunk for rendering.
        /// </summary>
        /// <param name="atlas">The 2D array of tile IDs</param>
        /// <param name="size">The size of the tiles.</param>
        public void Generate(int[,] atlas, int size) {
            //Initialize the collision array
            Collisions = new TileCollision[atlas.GetLength(0), atlas.GetLength(1)];

            //Generate the offset so the chunk is added at the correct height
            int yoffset = 0;
            for (int iter = 0; iter < atlas.GetLength(0); iter++) {
                //If atlas value is equal to start tile
                if (atlas[iter, 0] == 4) yoffset = iter;
                //y offset equals the iterator
            }
            //Iterate through each value of the Atlas
            for (int x = 0; x < atlas.GetLength(1); x++) {
                for (int y = 0; y < atlas.GetLength(0); y++) {
                    //Add a new tile to the tiles list with an ID and rect from the Atlas.
                    //Set the proper collision value for this tile based on its ID
                    TileCollision tc;

                    switch(atlas[y, x]) {
                        case 1:
                        case 4:
                        case 5:
                            tc = TileCollision.Impassable;
                            break;

                        case 6:
                            tc = TileCollision.Platform;
                            break;

                        default:
                            tc = TileCollision.Passable;
                            break;
                    }
                    //Set the collision matrix value for this tile
                    Collisions[y, x] = tc;

                    //Add the tile to the list, instantiate it
                    Tile tileToAdd = new Tile(atlas[y, x], new Microsoft.Xna.Framework.Rectangle(o.X + (x * size + size), o.Y + (y * size) - (yoffset * size), size, size), tc);

                    Tiles.Add(tileToAdd);

                    width = (x + 1) * size;
                    height = (y + 1) * size;
                }
            }

            //BETWEEN THIS LINE AND END OF METHOD = TILE PRETTY-IFYING (stitching, decorating)
            //Set adjacent tiles for each tile
            int i = 0; //increment through the array of tile IDs

            for (int x = 0; x < atlas.GetLength(1); x++) { //Execute for each tile
                for (int y = 0; y < atlas.GetLength(0); y++) {
                    int thisID = Tiles[i].ID; //Current tile ID
                    //if this isn't an air tile (which we don't stitch)
                    if (thisID != 0) {
                        bool stitchOnlySameID = true; //For stitching background tiles into foreground tiles

                        if (thisID == 2) //If this is a background tile, need to stitch it into foreground, but not vice versa
                            stitchOnlySameID = false;

                        int bkdCount = 0; //For stitching background tiles into foreground tiles

                        //Construct the Adjacents array for this tile
                        //(i.e. add the cardinal tile IDs to this tile's Adjacents array in the order North West East South
                        if (y - 1 >= 0 && (atlas[y - 1, x] == thisID || 
                            (!stitchOnlySameID && atlas[y - 1, x] != 0) ||
                            (thisID == 1 || thisID == 4 || thisID == 5) && (atlas[y - 1, x] == 4 || atlas[y - 1, x] == 5 || atlas[y - 1, x] == 1)))
                            Tiles[i].Adjacents[0] = true;
                        else if (y - 1 >= 0 && atlas[y - 1, x] == 2) //If this cardinal has a background tile...
                            bkdCount++; //increment the count of adjacent background tiles

                        if (x - 1 >= 0 && (atlas[y, x - 1] == thisID || 
                            (!stitchOnlySameID && atlas[y, x - 1] != 0) ||
                            (thisID == 1 || thisID == 4 || thisID == 5) && (atlas[y, x - 1] == 4 || atlas[y, x - 1] == 5 || atlas[y, x - 1] == 1)))
                            Tiles[i].Adjacents[1] = true;
                        else if (x - 1 >= 0 && (atlas[y, x - 1] == 2))
                            bkdCount++;

                        if (x + 1 < atlas.GetLength(1) && (atlas[y, x + 1] == thisID || 
                            (!stitchOnlySameID && atlas[y, x + 1] != 0) ||
                            (thisID == 1 || thisID == 4 || thisID == 5) && (atlas[y, x + 1] == 4 || atlas[y, x + 1] == 5 || atlas[y, x + 1] == 1)))
                            Tiles[i].Adjacents[2] = true;
                        else if (x + 1 < atlas.GetLength(1) && (atlas[y, x + 1] == 2))
                            bkdCount++;

                        if (y + 1 < atlas.GetLength(0) && (atlas[y + 1, x] == thisID || 
                            (!stitchOnlySameID && atlas[y + 1, x] != 0) ||
                            (thisID == 1 || thisID == 4 || thisID == 5) && (atlas[y + 1, x] == 4 || atlas[y + 1, x] == 5 || atlas[y + 1, x] == 1)))
                            Tiles[i].Adjacents[3] = true;
                        else if (y + 1 < atlas.GetLength(0) && (atlas[y + 1, x] == 2))
                            bkdCount++;

                        //If more than 1 background tile borders this foreground tile...
                        if (bkdCount > 1)
                            Tiles.Add(Tiles[i].AddBackgroundTile()); //...add another background tile behind it to fill gaps  
                    }
                    
                    i++; //Advance the tile list
                }
            }

            //Generate the decorations and start/end tile information for each tile
            foreach (Tile tile in Tiles) {
                if (tile.ID == 4)
                    StartTile = tile;
                else if (tile.ID == 5)
                    EndTile = tile;
                tile.GenerateDecorations();
            }

            Generated = true;
        }

        //Returns the collision at the tile of row x and column y in this chunk
        public TileCollision GetCollision(int x, int y) {
            // Prevent escaping past the level ends.
            if (x < 0 || x >= Width)
                return TileCollision.Impassable;
            // Allow jumping past the level top and falling through the bottom.
            if (y < 0 || y >= Height)
                return TileCollision.Passable;

            return Collisions[x, y];
        }

        /// <summary>
        /// Gets the bounding rectangle of a tile in world space.
        /// </summary>        
        public Rectangle GetBounds(int x, int y) {
            int size = Tiles[0].Rect.Width;
            return new Rectangle(x * size, y * size, size, size);
        }

        public void Draw(SpriteBatch sb, Microsoft.Xna.Framework.Color color) {
            //The tiles are sorted and drawn in ascending order of the DrawQueue property value
            List<Tile> sortedTiles = Tiles.OrderBy(o => o.DrawQueue).ToList();
            foreach (Tile tile in sortedTiles)
                tile.Draw(sb, color);
        }

        /// <summary>
        /// Reads in an image from a given path relative to the directory .\Content\maps\ (of the executable) and converts it to a 2D array of tile IDs.
        /// </summary>
        /// <param name="imagePath">The relative path of the image, starting from .\Content\maps\ in the application's current directory.</param>
        /// <returns>A 2D array of tile IDs.</returns>
        public int[,] ReadImage(string imagePath) {
            /*string path = Directory.GetCurrentDirectory();
            string newPath = Path.GetFullPath(Path.Combine(path, @".\Content\maps\" + imagePath)); //THIS PATH MAY NEED TO BE AMENDED IN THE FUTURE*/
            Bitmap img = new Bitmap(Path.Combine(@".\Content\maps\"+imagePath)); //Get and convert the file to a readable bitmap.

            int[,] atlas = new int[img.Height, img.Width]; //Initialize dimensions of returned array

            for(int x = 0; x < atlas.GetLength(1); x++) { //Iterate through each atlas index
                for(int y = 0; y < atlas.GetLength(0); y++) {
                    Color px = img.GetPixel(x, y); //Get the pixel color at this index and convert it to a string

                    string pxStr = 
                        px.R.ToString("D3") + " " +
                        px.G.ToString("D3") + " " +
                        px.B.ToString("D3") + " " +
                        px.A.ToString("D3");
                    
                    atlas[y, x] = TileID(pxStr, x, img.Width); //Convert the pixel color to an integer ID and populate the array
                }
            }

            return atlas; //Return the complete array
        }

        /// <summary>
        /// Returns the integer tile ID from a given color, the relative x position within the chunk, and the width of the chunk.
        /// </summary>
        /// <param name="color">The color of the tile, in RGBA "### ### ### ###" format.</param>
        /// <param name="x">The x position of the tile within the chunk.</param>
        /// <param name="width">The width of the chunk.</param>
        /// <returns>The integer tile ID.</returns>
        int TileID(string color, int x = -1, int width = -1) {
            int tileID = 0;

            switch (color) {
                case "092 186 072 255":
                    tileID = 1; //Ground tile
                    break;

                case "105 119 135 255":
                    tileID = 2; //Background tile
                    break;

                case "255 174 012 255":
                    tileID = 3; //Gold tile
                    break;

                case "255 000 000 255":
                    //use the chunk width to determine whether this red pixel is on the left side or the right side of the atlas
                    if (x == 0)
                        tileID = 4; //Start tile (grass tile with bool isStartTile = true)
                    else if (x == width - 1) 
                        tileID = 5; //End tile (grass tile with bool isEndTile = true)
                    else
                        tileID = 1; //Otherwise, the red tile is extraneous and is treated as a regular grass tile
                    break;
            }
            return tileID;
        }
    }
}