using FarseerPhysics.Dynamics;
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

        public PhysicsObject physics;

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

        public Tile(int ID, Rectangle r) {
            
            this.ID = ID;
            Rect = r;

            if (ID == 1 || ID == 3 || ID == 4 || ID == 5) { //Draw queue means background tiles are rendered behind ground tiles
                DrawQueue = 0;
                texture = Content.Load<Texture2D>("tile_1");
                //physics = new PhysicsObject(new Vector2(Rect.Size.X, Rect.Size.Y), 1f, "ground");
                physics = new PhysicsObject(Rect.Size.X, Rect.Size.Y, 1f, "ground");
                physics.Position = new Vector2(Rect.X, Rect.Y);
            } else {
                DrawQueue = -1;
                texture = Content.Load<Texture2D>("tile_" + ID);
            }

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

            if (physics != null) {
                Rectangle destination = new Rectangle
                (
                    (int)physics.Position.X,
                    (int)physics.Position.Y,
                    (int)physics.Size.X,
                    (int)physics.Size.Y
                );

                sb.Draw(texture, destination, null, Color.White, physics.Body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
            } else {
                sb.Draw(texture, Rect, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
            }

            foreach (TileDecoration d in Decorations)
                d.Draw(sb);
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
                
                if(ID == 4 || ID == 5) //Start and end tiles should be textured as Grass
                    Texture = Content.Load<Texture2D>(@"graphics\tile\tile_1_" + bmv);
                else //Other tiles are textured according to their IDs
                    Texture = Content.Load<Texture2D>(@"graphics\tile\tile_" + ID + "_" + bmv);
            }

            stitched = true;
        }

        public Tile AddBackgroundTile() {
            return new Tile(Content.Load<Texture2D>(@"graphics\tile\tile_2_15"), Rect);
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
