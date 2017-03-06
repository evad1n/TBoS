using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TheBondOfStone {
    /// <summary>
    /// Singleton tile sprite manager.
    /// </summary>
    sealed class TileSpriteManager {

        Dictionary<int, Texture2D[]> textures;

        private TileSpriteManager() {

        }
        private static TileSpriteManager manager;
        public static TileSpriteManager Manager {
            get {
                if (manager == null) manager = new TileSpriteManager();
                return manager;
            }
        }

        public Texture2D this[int ID, int bitmap] {
            get {
                return textures[ID][bitmap];
            }
        }

        /// <summary>
        /// Initializes the tile texture array with all tiles.  Requires 16 unique files for each tile ID.
        /// </summary>
        /// <param name="path">The relative path to the directory containing the tiles.  Preceding .\\ is optional.</param>
        /// <param name="IDs">The integer values of valid tile texture files.</param>
        public void InitializeTextures(String path, int[] IDs, ContentManager cm) {
            path = Path.Combine(@".\", path);
            if (Directory.Exists(path)) {
                foreach(int i in IDs) {
                    textures.Add(i, new Texture2D[16]);
                    for(int mask = 0; mask < 16; mask++) {
                        String filename = "tile_" + i + "_" + mask; //May be useful to change this over to be variably specified in case we want to change the format later.
                        textures[i][mask] = cm.Load<Texture2D>(filename);
                    }
                }
            }
            else throw new NullReferenceException("File not found.");
        }
    }
}
