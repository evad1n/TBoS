using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    public enum FacingDirection { Up, Left, Down, Right };

    /// <summary>
    /// A static hazard which hurts the player if it comes in contact with the "business end"
    /// 
    /// By Dom Liotti
    /// </summary>
    class Spike : Entity
    {
        public FacingDirection Facing;
        public Rectangle HitRect;

        public Spike(Texture2D texture, Vector2 position, FacingDirection direction) : base(texture, position) {
            Texture = texture;
            Position = position;

            Facing = direction;

            if (Facing == FacingDirection.Up) {
                Texture = Graphics.Spike_Up[Game1.RandomObject.Next(0, Graphics.Spike_Up.Length)];
                Position = new Vector2(Position.X, Position.Y + Game1.PIXEL_SCALE);
                HitRect = new Rectangle(Rect.X, Rect.Y - 2 * Game1.PIXEL_SCALE, Rect.Width, Rect.Height + 2 * Game1.PIXEL_SCALE + 1);
            } else if (Facing == FacingDirection.Left) {
                Texture = Graphics.Spike_Left[Game1.RandomObject.Next(0, Graphics.Spike_Left.Length)];
                Position = new Vector2(Position.X + Game1.PIXEL_SCALE, Position.Y);
                HitRect = new Rectangle(Rect.X - Game1.PIXEL_SCALE, Rect.Y, Rect.Width + Game1.PIXEL_SCALE, Rect.Height);
            } else if (Facing == FacingDirection.Down) {
                Texture = Graphics.Spike_Down[Game1.RandomObject.Next(0, Graphics.Spike_Down.Length)];
                Position = new Vector2(Position.X, Position.Y - Game1.PIXEL_SCALE);
                HitRect = new Rectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height + Game1.PIXEL_SCALE);
            } else if (Facing == FacingDirection.Right) {
                Texture = Graphics.Spike_Right[Game1.RandomObject.Next(0, Graphics.Spike_Right.Length)];
                Position = new Vector2(Position.X - Game1.PIXEL_SCALE, Position.Y);
                HitRect = new Rectangle(Rect.X, Rect.Y, Rect.Width + Game1.PIXEL_SCALE, Rect.Height);
            }
        }

        //Overloaded to draw with rotation
        public override void Draw(SpriteBatch spriteBatch, Color color)
        {
            //If this is active, draw it.
            if (Active)
            {
                //We can "lock" entities to the virtual pixel grid (looks pretty nice)
                if (LockToPixelGrid)
                {
                    Rectangle drawRect = new Rectangle(
                        (int)Math.Round(Position.X / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                        (int)Math.Round(Position.Y / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                        Texture.Width * Game1.PIXEL_SCALE,
                        Texture.Height * Game1.PIXEL_SCALE);

                    spriteBatch.Draw(
                        texture: Texture, 
                        destinationRectangle: drawRect, 
                        color: color);
                }
                else
                    spriteBatch.Draw(
                        texture: Texture,
                        destinationRectangle: Rect,
                        color: color);
            }
        }
    }
}
