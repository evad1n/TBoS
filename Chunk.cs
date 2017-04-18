using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    /// <summary>
    /// Represents a chunk's tile data, and is capable of drawing and generating a chunk from image data.
    /// 
    /// By Dom Liotti and Chip Butler
    /// </summary>
    public class Chunk {
        //Has the chunk been stitched to the chunk before it?
        private bool stitched;

        /// <summary>
        /// Keeps track of whether the chunk has been stitched to the *previous* chunk.
        /// </summary>
        public bool Stitched {
            get { return stitched; }
        }


        //Linear list of tile data.
        public List<Tile> Tiles = new List<Tile>();

        //List of the tile IDs within the chunk, in a 2D array (for stitching and similar)
        private int[,] atlas;

        //coordinate-to-Tile finder dictionary
        private Dictionary<int[], Tile> TileByAtlas;

        /// <summary>
        /// The width of the chunk in tiles.
        /// </summary>
        public int AtlasWidth { get { return atlas.GetLength(1); } }
        /// <summary>
        /// The height of the chunk in tiles.
        /// </summary>
        public int AtlasHeight { get { return atlas.GetLength(0); } }

        /// <summary>
        /// The coordinates of the start tile, in Y X format.
        /// </summary>
        private int[] startTileCoords;

        /// <summary>
        /// The coordinates of the start tile in Y X order.  (as in, [0] contains the y coordinate, [1] contains the x coordinate.  
        /// </summary>
        public int[] StartTileCoords {
            get {
                return startTileCoords;
            }
        }

        /// <summary>
        /// The coordinates of the end tile, in Y X format.
        /// </summary>
        private int[] endTileCoords;

        /// <summary>
        /// The coordinates of the end tile in Y X order.  (as in, [0] contains the y coordinate, [1] contains the x coordinate.  
        /// </summary>
        public int[] EndTileCoords {
            get {
                return endTileCoords;
            }
        }


        //Accessor for the list of tile IDs.
        public int this [int y, int x] {
            get { return atlas[y, x]; }
        }

        //Linear list of entity data.
        public List<Entity> Entities = new List<Entity>();

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
            stitched = true;
            atlas = MapReader.ReadImage(path);

            this.origin = origin;
            Generate(atlas, Game1.TILE_SIZE);
        }

        //Regular chunk instantiation
        public Chunk(Rectangle origin) {
            stitched = false;
            this.origin = origin;
        }

        /// <summary>
        /// Generates the chunk for rendering.
        /// </summary>
        /// <param name="atlas">The 2D array of tile IDs</param>
        /// <param name="size">The size of the tiles.</param>
        public void Generate(int[,] atlas, int size) {
            this.atlas = atlas;
            //Generate the offset so the chunk is added at the correct height
            int yoffset = 0;
            for (int iter = 0; iter < atlas.GetLength(0); iter++) {
                bool foundStartTile = false;
                //If atlas value is a start tile
                if (atlas[iter, 0] == 4) {
                    yoffset = iter;
                    startTileCoords = new int[] { yoffset, 0 };
                    foundStartTile = true;
                }
                //y offset equals the iterator
                if (foundStartTile) break;
            }

            for (int iter = 0; iter < atlas.GetLength(0); iter++)
            {
                bool foundEndTile = false;
                if (atlas[iter, atlas.GetLength(1) - 1] == 5)
                {
                    endTileCoords = new int[] { iter, atlas.GetLength(1) };
                    foundEndTile = true;
                }
                if (foundEndTile) break;
            }

            TileByAtlas = new Dictionary<int[], Tile>(atlas.Length);

            //Iterate through each value of the Atlas
            for (int x = 0; x < atlas.GetLength(1); x++) {
                for (int y = 0; y < atlas.GetLength(0); y++) {

                    //Add the tile to the list, instantiate it
                    Tile tileToAdd = new Tile(atlas[y, x], new Rectangle(origin.X + (x * size + size), origin.Y + (y * size) - (yoffset * size), size, size));

                    //ENTITY SPAWNING
                    if (atlas[y, x] == 6)
                    {
                        tileToAdd.ID = atlas[y, x] = 0;
                        Entities.Add(new CoinPickup(Graphics.PickupTexture_Coin[0], new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), 1));
                    }
                    else if (atlas[y, x] == 7)
                    {
                        tileToAdd.ID = atlas[y, x] = 2;
                        Entities.Add(new CoinPickup(Graphics.PickupTexture_Coin[0], new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), 1));
                    } else if (atlas[y, x] == 18) {
                        tileToAdd.ID = atlas[y, x] = 0;
                        Entities.Add(new HealthPickup(Graphics.PickupTexture_Health[0], new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), 2));
                    } else if (atlas[y, x] == 19) {
                        tileToAdd.ID = atlas[y, x] = 2;
                        Entities.Add(new HealthPickup(Graphics.PickupTexture_Health[0], new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), 2));
                    } else if (atlas[y, x] == 8) {
                        tileToAdd.ID = atlas[y, x] = 0;
                        Game1.Entities.enemies.Add(new GroundEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size))));
                    }
                    else if (atlas[y, x] == 9)
                    {
                        tileToAdd.ID = atlas[y, x] = 2;
                        Game1.Entities.enemies.Add(new GroundEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size))));
                    }
                    else if (atlas[y, x] == 14)
                    {
                        tileToAdd.ID = atlas[y, x] = 0;
                        Game1.Entities.enemies.Add(new JumpingEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size))));
                    }
                    else if (atlas[y, x] == 15)
                    {
                        tileToAdd.ID = atlas[y, x] = 2;
                        Game1.Entities.enemies.Add(new JumpingEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size))));
                    }
                    else if (atlas[y, x] == 16)
                    {
                        tileToAdd.ID = atlas[y, x] = 0;
                        Game1.Entities.enemies.Add(new FlyingEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), true));
                    }
                    else if (atlas[y, x] == 17)
                    {
                        tileToAdd.ID = atlas[y, x] = 2;
                        Game1.Entities.enemies.Add(new FlyingEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), true));
                    }
                    else if (atlas[y, x] == 20)
                    {
                        tileToAdd.ID = atlas[y, x] = 0;
                        Game1.Entities.enemies.Add(new FlyingEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), false));
                    }
                    else if (atlas[y, x] == 21)
                    {
                        tileToAdd.ID = atlas[y, x] = 2;
                        Game1.Entities.enemies.Add(new FlyingEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), false));
                    }
                    else if (atlas[y, x] == 22)
                    {
                        tileToAdd.ID = atlas[y, x] = 3;
                        Game1.Entities.enemies.Add(new TurretEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), Projectile.Arrow, new Vector2(0, -1)));
                    }
                    else if (atlas[y, x] == 23)
                    {
                        tileToAdd.ID = atlas[y, x] = 3;
                        Game1.Entities.enemies.Add(new TurretEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), Projectile.Arrow, new Vector2(1, 0)));
                    }
                    else if (atlas[y, x] == 24)
                    {
                        tileToAdd.ID = atlas[y, x] = 3;
                        Game1.Entities.enemies.Add(new TurretEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), Projectile.Arrow, new Vector2(0, 1)));
                    }
                    else if (atlas[y, x] == 25)
                    {
                        tileToAdd.ID = atlas[y, x] = 3;
                        Game1.Entities.enemies.Add(new TurretEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), Projectile.Arrow, new Vector2(-1, 0)));
                    }
                    else if (atlas[y, x] == 26)
                    {
                        tileToAdd.ID = atlas[y, x] = 3;
                        Entities.Add(new SpearTrap(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), new Vector2(0, -1)));
                    }
                    else if (atlas[y, x] == 27)
                    {
                        tileToAdd.ID = atlas[y, x] = 3;
                        Entities.Add(new SpearTrap(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), new Vector2(1, 0)));
                    }
                    else if (atlas[y, x] == 28)
                    {
                        tileToAdd.ID = atlas[y, x] = 3;
                        Entities.Add(new SpearTrap(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), new Vector2(0, 1)));
                    }
                    else if (atlas[y, x] == 29)
                    {
                        tileToAdd.ID = atlas[y, x] = 3;
                        Entities.Add(new SpearTrap(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), new Vector2(-1, 0)));
                    }             
                    else if (atlas[y, x] == 30)
                    {
                        tileToAdd.ID = atlas[y, x] = 0;
                        Game1.Entities.enemies.Add(new TurretEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), Projectile.Spear, Vector2.Zero));
                    }
                    else if (atlas[y, x] == 31)
                    {
                        tileToAdd.ID = atlas[y, x] = 2;
                        Game1.Entities.enemies.Add(new TurretEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), Projectile.Spear, Vector2.Zero));
                    }
                    else if (atlas[y, x] == 32)
                    {
                        tileToAdd.ID = atlas[y, x] = 2;
                        Game1.Entities.enemies.Add(new TurretEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), Projectile.Sawblade, new Vector2(-1, 0)));
                    }
                    else if (atlas[y, x] == 33)
                    {
                        tileToAdd.ID = atlas[y, x] = 2;
                        Game1.Entities.enemies.Add(new TurretEnemy(new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), Projectile.Sawblade, new Vector2(1, 0)));
                    }
                    else if (atlas[y, x] == 10 || atlas[y, x] == 12)
                    {
                        if (atlas[y, x] == 10) //adding a vertical spike
                            Entities.Add(new Spike(Graphics.Spike_Up[0], new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), GetSpikeRotation(false, x, y, atlas)));
                        else //adding a horizontal spike
                            Entities.Add(new Spike(Graphics.Spike_Up[0], new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), GetSpikeRotation(true, x, y, atlas)));

                        tileToAdd.ID = atlas[y, x] = 0;
                    }
                    else if (atlas[y, x] == 11 || atlas[y, x] == 13)
                    {
                        if (atlas[y, x] == 11) //adding a vertical spike
                            Entities.Add(new Spike(Graphics.Spike_Up[0], new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), GetSpikeRotation(false, x, y, atlas)));
                        else //adding a horizontal spike
                            Entities.Add(new Spike(Graphics.Spike_Up[0], new Vector2(origin.X + (x * size + size / 2), origin.Y + (y * size - size / 2) - (yoffset * size)), GetSpikeRotation(true, x, y, atlas)));

                        tileToAdd.ID = atlas[y, x] = 2;
                    }

                    Tiles.Add(tileToAdd);
                    TileByAtlas.Add(new int[] { y, x }, tileToAdd);

                    rect = new Rectangle(
                        origin.X, 
                        origin.Y + (size / 2) - (yoffset * size),
                        (x + 1) * size,
                        (y + 1) * size
                        );

                }
            }

            foreach(Tile t in Tiles)
            {
                t.SetDrawQueue();
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

        FacingDirection GetSpikeRotation(bool horizontal, int x, int y, int[,] atlas) {
            bool[] adjacents = new bool[4];

            if (y - 1 >= 0 && CollisionHelper.IsSolidTile(atlas[y - 1, x]))
                adjacents[0] = true;
            if (x - 1 >= 0 && CollisionHelper.IsSolidTile(atlas[y, x - 1]))
                adjacents[2] = true;
            if (x + 1 < atlas.GetLength(1) && CollisionHelper.IsSolidTile(atlas[y, x + 1]))
                adjacents[1] = true;
            if (y + 1 < atlas.GetLength(0) && CollisionHelper.IsSolidTile(atlas[y + 1, x]))
                adjacents[3] = true;

            if (horizontal) {
                if (adjacents[2])
                    return FacingDirection.Right;
                else if (adjacents[1])
                    return FacingDirection.Left;
                return 0;
            } else {
                if (adjacents[0])
                    return FacingDirection.Down;
                else if (adjacents[3])
                    return FacingDirection.Up;
                return FacingDirection.Up;
            }
        }

        public void Update(GameTime gameTime) {
            List<Entity> garbageEntities = new List<Entity>();
            //Update static entities
            foreach(Entity e in Entities) {
                if (!e.Active)
                    garbageEntities.Add(e);

                if(e is CoinPickup) {
                    CoinPickup c = (CoinPickup)e;
                    c.Update(gameTime);
                }

                if (e is HealthPickup) {
                    HealthPickup h = (HealthPickup)e;
                    h.Update(gameTime);
                }
            }

            foreach (Entity e in garbageEntities)
                Entities.Remove(e);
        }

        public void DrawForeground(SpriteBatch sb, Color color) {
            //The tiles are sorted and drawn in ascending order of the DrawQueue property value
            List<Tile> foreground = Tiles.Where(s => s.DrawQueue == 1).ToList();
            foreach (Tile tile in foreground)
                tile.Draw(sb, color);


            //Draw chunks
            //sb.Draw(Graphics.DebugTexture, rect, color);
        }

        public void DrawBackground(SpriteBatch sb, Color color)
        {
            //The tiles are sorted and drawn in ascending order of the DrawQueue property value
            List<Tile> background = Tiles.Where(s => s.DrawQueue == 0).ToList();
            foreach (Tile tile in background)
                tile.Draw(sb, color);

            if (Entities.Count > 0)
            {
                foreach (Entity e in Entities)
                    e.Draw(sb, Color.White);
            }

            //Draw chunks
            //sb.Draw(Graphics.DebugTexture, rect, color);
        }

        /// <summary>
        /// Stitches two chunks together.
        /// </summary>
        /// <param name="left">The chunk to the left</param>
        /// <param name="right">The chunk to the right</param>
        public static void StitchChunks(Chunk left, Chunk right) {
            right.stitched = true;
            for(int i = left.EndTileCoords[0]-1, j = right.StartTileCoords[0]-1; i > -1 && j > -1; i--, j--) {
                if((left[i,left.AtlasWidth-1] == 1 || left[i, left.AtlasWidth - 1] == 3 || left[i, left.AtlasWidth - 1] == 4 || left[i, left.AtlasWidth - 1] == 5) &&(right[j, 0] == 1 || right[j,0] == 3 || right[j,0] == 4 || right[j,0] == 5)) {
                    Tile tmp = left.TileByAtlas.SingleOrDefault(t => (t.Key.First()==i) && (t.Key.Last() == left.AtlasWidth - 1)).Value;
                    tmp.Adjacents[2] = true;
                    tmp.StitchTile();
                    tmp = right.TileByAtlas.SingleOrDefault(t => (t.Key.First() == j) && (t.Key.Last() == 0)).Value;
                    tmp.Adjacents[1] = true;
                    tmp.StitchTile();
                }
            }
            for (int i = left.EndTileCoords[0] + 1, j = right.StartTileCoords[0] + 1; i < left.AtlasHeight && j < right.AtlasHeight; i++, j++) {
                if ((left[i, left.AtlasWidth - 1] == 1 || left[i, left.AtlasWidth - 1] == 3 || left[i, left.AtlasWidth - 1] == 4 || left[i, left.AtlasWidth - 1] == 5) && (right[j, 0] == 1 || right[j, 0] == 3 || right[j, 0] == 4 || right[j, 0] == 5)) {
                    Tile tmp = left.TileByAtlas.SingleOrDefault(t => (t.Key.First() == i) && (t.Key.Last() == left.AtlasWidth - 1)).Value;
                    tmp.Adjacents[2] = true;
                    tmp.StitchTile();
                    tmp = right.TileByAtlas.SingleOrDefault(t => (t.Key.First() == j) && (t.Key.Last() == 0)).Value;
                    tmp.Adjacents[1] = true;
                    tmp.StitchTile();
                }
            }
        }
    }
}
