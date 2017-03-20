using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    /// <summary>
    /// Represents a chunk's tile data.
    /// </summary>
    public class Chunk {
        //Linear list of tile data.
        public List<Tile> Tiles = new List<Tile>();

        //References to the start and end tiles of this chunk.
        public Tile StartTile { get; set; }
        public Tile EndTile { get; set; }

        //Is this chunk generated from its data yet?
        public bool Generated { get; set; }

        //Bounding rectangle of this chunk.
        private Rectangle rect;
        public Rectangle Rect
        {
            get { return rect; }
        }

        //Quick reference to the right and bottom bounds of the chunk.
        public int Right { get { return Rect.X + Rect.Width; } }
        public int Bottom { get { return Rect.Y + Rect.Height; } }

        //The origin of this chunk (The rect of the start tile).
        Rectangle origin;

        //Starter generation
        public Chunk(string path, Rectangle origin) {
            int[,] atlas = MapReader.ReadImage(path);

            this.origin = origin;
            Generate(atlas, Game1.TILE_SIZE);
        }

        //Regular chunk instantiation
        public Chunk(Rectangle origin) {
            this.origin = origin;
        }

        /// <summary>
        /// Generates the chunk for rendering.
        /// </summary>
        /// <param name="atlas">The 2D array of tile IDs</param>
        /// <param name="size">The size of the tiles.</param>
        public void Generate(int[,] atlas, int size) {
            //Generate the offset so the chunk is added at the correct height
            int yoffset = 0;
            for (int iter = 0; iter < atlas.GetLength(0); iter++) {
                //If atlas value is a start tile
                if (atlas[iter, 0] == 4) yoffset = iter;
                //y offset equals the iterator
            }
            //Iterate through each value of the Atlas
            for (int x = 0; x < atlas.GetLength(1); x++) {
                for (int y = 0; y < atlas.GetLength(0); y++) {

                    //Add the tile to the list, instantiate it
                    Tile tileToAdd = new Tile(atlas[y, x], new Rectangle(origin.X + (x * size + size), origin.Y + (y * size) - (yoffset * size), size, size));

                    Tiles.Add(tileToAdd);

                    rect = new Rectangle(
                        origin.X,
                        origin.Top,
                        (x + 1) * size,
                        (y + 1) * size
                        );
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
                        int adjCount = 0;

                        //Construct the Adjacents array for this tile
                        //(i.e. add the cardinal tile IDs to this tile's Adjacents array in the order North West East South
                        if (y - 1 >= 0 && (atlas[y - 1, x] == thisID ||
                            (!stitchOnlySameID && atlas[y - 1, x] != 0) ||
                            (thisID == 1 || thisID == 3 || thisID == 4 || thisID == 5) && (atlas[y - 1, x] == 1 || atlas[y - 1, x] == 3 || atlas[y - 1, x] == 4 || atlas[y - 1, x] == 5)))
                            Tiles[i].Adjacents[0] = true;
                        else if (y - 1 >= 0 && atlas[y - 1, x] == 2) { //If this cardinal has a background tile...
                            bkdCount++; //increment the count of adjacent background tiles
                            adjCount++;
                        }

                        if (x - 1 >= 0 && (atlas[y, x - 1] == thisID ||
                            (!stitchOnlySameID && atlas[y, x - 1] != 0) ||
                            (thisID == 1 || thisID == 3 || thisID == 4 || thisID == 5) && (atlas[y, x - 1] == 1 || atlas[y, x - 1] == 3 || atlas[y, x - 1] == 4 || atlas[y, x - 1] == 5)))
                            Tiles[i].Adjacents[1] = true;
                        else if (x - 1 >= 0 && (atlas[y, x - 1] == 2)) {
                            bkdCount++;
                            adjCount++;
                        }

                        if (x + 1 < atlas.GetLength(1) && (atlas[y, x + 1] == thisID ||
                            (!stitchOnlySameID && atlas[y, x + 1] != 0) ||
                            (thisID == 1 || thisID == 3 || thisID == 4 || thisID == 5) && (atlas[y, x + 1] == 1 || atlas[y, x + 1] == 3 || atlas[y, x + 1] == 4 || atlas[y, x + 1] == 5)))
                            Tiles[i].Adjacents[2] = true;
                        else if (x + 1 < atlas.GetLength(1) && (atlas[y, x + 1] == 2)) {
                            bkdCount++;
                            adjCount++;
                        }

                        if (y + 1 < atlas.GetLength(0) && (atlas[y + 1, x] == thisID ||
                            (!stitchOnlySameID && atlas[y + 1, x] != 0) ||
                            (thisID == 1 || thisID == 3 || thisID == 4 || thisID == 5) && (atlas[y + 1, x] == 1 || atlas[y + 1, x] == 3 || atlas[y + 1, x] == 4 || atlas[y + 1, x] == 5)))
                            Tiles[i].Adjacents[3] = true;
                        else if (y + 1 < atlas.GetLength(0) && (atlas[y + 1, x] == 2)) {
                            bkdCount++;
                            adjCount++;
                        }

                        //If more than 1 background tile borders this foreground tile...
                        if (bkdCount > 1 && adjCount > 0)
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

        public void Draw(SpriteBatch sb, Color color) {
            //The tiles are sorted and drawn in ascending order of the DrawQueue property value
            List<Tile> sortedTiles = Tiles.OrderBy(o => o.DrawQueue).ToList();
            foreach (Tile tile in sortedTiles)
                tile.Draw(sb, color);
        }
    }
}
