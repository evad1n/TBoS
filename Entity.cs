using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    /// <summary>
    /// Basic extendable "not-a-tile" class. Draws a texture and has a rectangle.
    /// 
    /// By Dom Liotti and Will Dickinson
    /// </summary>
    public class Entity {

        //The entity's texture
        public Texture2D texture;
        public Texture2D Texture {
            get { return texture; }
            set { texture = value; }
        }

        public bool Active { get; set; }

        //The entity's screen position
        Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; } }

        //Constructs the rectangle represented by the entity ('hitbox' is always the size of a tile)
        public Rectangle Rect {
            get {
                return new Rectangle(
                    (int)Position.X, 
                    (int)Position.Y, 
                    Texture.Width * Game1.PIXEL_SCALE, 
                    Texture.Height * Game1.PIXEL_SCALE
                    );
            }
        }

        //The chunk that the entity currently occupies (where is it on X relative to the chunks' bounds?)
        Chunk currentChunk;
        public Chunk CurrentChunk {
            get { return Game1.Generator.GetEntityChunkID(this); }
            set { currentChunk = value; }
        }

        public bool LockToPixelGrid;

        public Entity(Texture2D texture, Vector2 position, bool locktopixelgrid = true, bool active = true) {
            Texture = texture;
            Position = position;
            LockToPixelGrid = locktopixelgrid;
            Active = active;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Color color) {
            //If this is active, draw it.
            if (Active) {
                //We can "lock" entities to the virtual pixel grid (looks pretty nice)
                if (LockToPixelGrid) {
                    Rectangle drawRect = new Rectangle(
                        (int)Math.Round(Position.X / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                        (int)Math.Round(Position.Y / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                        Texture.Width * Game1.PIXEL_SCALE,
                        Texture.Height * Game1.PIXEL_SCALE
                        );

                    spriteBatch.Draw(Texture, drawRect, color);
                } else
                    spriteBatch.Draw(Texture, Rect, color);
            }
        }

        /// <summary>
        /// Check collision of this entity with another entity.
        /// </summary>
        /// <param name="other">The entity to check collision with.</param>
        /// <returns>True if the two entities' rectangles are intersecting.</returns>
        public bool CollidesWith(Entity other) {
            return Active && other.Active && other.Rect.Intersects(Rect);
        }
    }
}