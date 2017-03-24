using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    class Spike : Entity
    {
        float rotation;
        string rotString;

        public Spike(Texture2D texture, Vector2 position, string rotation) : base(texture, position) {
            Texture = texture;
            Position = position;

            rotString = rotation;
            FindTextureAndRotation(rotation);
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
                        color: color,
                        rotation: rotation);
                }
                else
                    spriteBatch.Draw(
                        texture: Texture,
                        destinationRectangle: Rect,
                        color: color,
                        rotation: rotation);
            }
        }

        //Get the correct texture based on the rotation
        void FindTextureAndRotation(string param)
        {
            switch (param)
            {
                case "up":
                    Texture = Graphics.HazardTextures[0];
                    rotation = 0f;
                    break;
                case "down":
                    Texture = Graphics.HazardTextures[0];
                    rotation = (float)(Math.PI);
                    break;
                case "left":
                    Texture = Graphics.HazardTextures[0];
                    rotation = (float)(Math.PI / 2);
                    break;
                case "right":
                    Texture = Graphics.HazardTextures[0];
                    rotation = (float)(Math.PI * 3 / 2);
                    break;
                case "upleft":
                    Texture = Graphics.HazardTextures[1];
                    rotation = (float)(Math.PI / 2);
                    break;
                case "upright":
                    Texture = Graphics.HazardTextures[1];
                    rotation = 0f;
                    break;
                case "downleft":
                    Texture = Graphics.HazardTextures[1];
                    rotation = (float)(Math.PI);
                    break;
                case "downright":
                    Texture = Graphics.HazardTextures[1];
                    rotation = (float)(Math.PI * 3 / 2);
                    break;
                default:
                    Texture = Graphics.HazardTextures[0];
                    rotation = 0f;
                    break;
            }
        }
    }
}
