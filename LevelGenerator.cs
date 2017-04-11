using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    /// <summary>
    /// Main engine for the world generation. Manages chunks and generation data.
    /// 
    /// By Dom Liotti and Will Dickinson
    /// </summary>
    public class LevelGenerator {
        Camera Camera;

        public List<Chunk> Chunks { get; set; }

        FileInfo[] mapFiles;

        Rectangle startRect;

        public LevelGenerator(GraphicsDeviceManager graphics, Rectangle startRect) {
            //Initialize the camera and chunk list
            Camera = Game1.Camera;
            this.startRect = startRect;
            Chunks = new List<Chunk>();

            //Get the count of maps in the maps folder
            DirectoryInfo mapDir = new DirectoryInfo(@"..\..\..\..\Content\maps");
            mapFiles = mapDir.GetFiles().Where(name => !name.Name.Contains("starter")).ToArray();
        }

        public void GenerateNewChunk(Rectangle startTileRect, string mapName) {
            Chunks.Add(new Chunk(startTileRect));
        }

        public void UpdateChunkGeneration() {
            Chunk prevChunk = Chunks[0];
            foreach (Chunk chunk in Chunks) {
                if (!chunk.Generated)
                    chunk.Generate(MapReader.ReadImage(GetNewMapName()), Game1.TILE_SIZE);
                if (!chunk.Stitched) {
                    Chunk.StitchChunks(prevChunk, chunk);
                }
                prevChunk = chunk;
            }

            if (Chunks.Count > 0) {
                if (Chunks[Chunks.Count - 1].EndTile.Rect.X < Camera.Rect.Right) {
                    //End tile of last chunk is on screen, generate new chunk
                    GenerateNewChunk(Chunks[Chunks.Count - 1].EndTile.Rect, GetNewMapName());
                }

                if (Chunks[0].EndTile.Rect.X <= Camera.Rect.Left - Chunks[0].EndTile.Rect.Width) {
                    //last tile of first chunk is off screen, destroy that chunk (i.e. remove it from the list).
                    Chunks.RemoveAt(0);
                }
            }
        }

        //Gets a random map name from the directory of the maps
        string GetNewMapName() {
            //Return the name of a file from mapFiles (the directory of the maps) 
            return mapFiles[Game1.RandomObject.Next(mapFiles.Length)].Name;
        }

        public void DoStarterGeneration() {
            Chunks.Add(new Chunk("starter.png", startRect));
        }

        public void Restart() {
            Chunks.Clear();

            DoStarterGeneration();
        }

        //Returns the chunk that an entity is 'in,' AKA the "current level"
        public Chunk GetEntityChunkID(Entity obj) {
            Vector2 r = obj.Position;
            foreach(Chunk c in Chunks) {
                if (r.X >= c.Rect.Left && r.X <= c.Rect.Right)
                {
                    return c;
                }
            }
            return null;
        }

        //Returns the chunk that a position is 'in,' AKA the "current level"
        public Chunk GetEntityChunkID(Rectangle rect)
        {
            Vector2 r = new Vector2(rect.X, rect.Y);
            foreach (Chunk c in Chunks)
            {
                if (r.X >= c.Rect.Left && r.X <= c.Rect.Right)
                {
                    return c;
                }
            }
            return null;
        }

        //Returns the chunk that a position is 'in,' AKA the "current level"
        public Chunk GetEntityChunkID(Vector2 pos)
        {
            foreach (Chunk c in Chunks)
            {
                if (pos.X >= c.Rect.Left && pos.X <= c.Rect.Right)
                {
                    return c;
                }
            }
            return null;
        }
    }
}
