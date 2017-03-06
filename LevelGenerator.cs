using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBondOfStone
{
    class LevelGenerator
    {

        public List<Chunk> Chunks { get; set; }

        FileInfo[] mapFiles;

        Camera camera;

        public LevelGenerator(Camera gameCamera)
        {
            //Initialize the camera and chunk list
            camera = gameCamera;
            Chunks = new List<Chunk>();

            //Get the count of maps in the maps folder
            DirectoryInfo mapDir = new DirectoryInfo(@".\Content\maps");
            mapFiles = mapDir.GetFiles();
        }

        public void GenerateNewChunk(Rectangle startTileRect, string mapName)
        {
            Chunks.Add(new Chunk(startTileRect, Game1.world));
        }

        public void UpdateChunkGeneration()
        {
            foreach (Chunk chunk in Chunks)
            {
                if (!chunk.Generated)
                    chunk.Generate(chunk.ReadImage(GetNewMapName()), Game1.PixelScaleFactor);
            }


            if (Chunks.Count > 0)
            {
                if (Chunks[0].EndTile.Rect.X <= camera.rect.Left - Chunks[0].EndTile.Rect.Width)
                {
                    //last tile of first chunk is off screen, destroy that chunk (i.e. remove it from the list).
                    Chunks.RemoveAt(0);
                }

                if (Chunks[Chunks.Count - 1].EndTile.Rect.X < camera.rect.Right) //This needs to updated to use the camera bounding box
                {
                    //End tile of last chunk is on screen, generate new chunk
                    GenerateNewChunk(Chunks[Chunks.Count - 1].EndTile.Rect, GetNewMapName());
                }
            }
        }

        //Gets a random map name from the directory of the maps
        string GetNewMapName()
        {
            //Return the name of a file from mapFiles (the directory of the maps) 
            return mapFiles[Game1.RandomObject.Next(mapFiles.Length)].Name;
        }

        public void DoStarterGeneration()
        {
            Chunks.Add(new Chunk());
        }

        public void Restart()
        {
            Chunks.Clear();
            DoStarterGeneration();
        }
    }
}
