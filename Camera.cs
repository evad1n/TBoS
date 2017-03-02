using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBondOfStone
{
    class Camera : Camera2D
    {
        //The target player of this Camera.
        Player target;
        //The left-translation-speed of the Camera.
        float speed = 0.5f;

        //The smoothing factor of the camera's follow behavior (greater values = slower following)
        float smoothing = 2f;

        Vector2 actualLookVector;
        Vector2 actualOrigin;

        //Shaking parameters
        float shakeTimer;
        float duration; 
        float shakeQuake;

        public Camera(GraphicsDevice graphicsDevice, Player target) : base(graphicsDevice)
        {
            this.target = target;

            actualLookVector = new Vector2(Origin.X, target.physicsRect.Position.Y);
            actualOrigin = new Vector2(Origin.X + speed, Origin.Y);
        } 

        public void Update(GameTime gameTime)
        {
            //Player follow code. Snaps to pixels.
            //Get the "actual" origin and position.
            actualLookVector = new Vector2(actualOrigin.X, target.physicsRect.Position.Y);
            actualOrigin = new Vector2(actualOrigin.X + speed, actualOrigin.Y);

            //Snap the camera's view to the pixel grid as per Game1.PixelScaleFactor
            LookAt(new Vector2(
                (int)(Math.Round(actualLookVector.X * Game1.PixelScaleFactor) / Game1.PixelScaleFactor), 
                (int)(Math.Round(actualLookVector.Y * Game1.PixelScaleFactor) / Game1.PixelScaleFactor)
                ));
            Origin = new Vector2(
                (int)(Math.Round(actualOrigin.X * Game1.PixelScaleFactor) / Game1.PixelScaleFactor),
                (int)(Math.Round(actualOrigin.Y * Game1.PixelScaleFactor) / Game1.PixelScaleFactor)
                );

            //Screen shake code
            if (shakeTimer < duration) {
                shakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                //Change the screen's position by either -shakeQuake or +shakeQuake each frame on each axis, and dampen shakeQuake.
                Position = new Vector2(Position.X + Game1.RandomObject.Next(-1, 2) * shakeQuake, Position.Y + Game1.RandomObject.Next(-1, 2) * shakeQuake);
                shakeQuake = Lerp(shakeQuake, 0, shakeTimer / duration);
            }
        }

        /// <summary>
        /// Shakes the screen from the default point at a given magnitude for a given duration.
        /// </summary>
        /// <param name="magnitude">The initial magnitude of the quake in (units)</param>
        /// <param name="duration">The duration of the quake in (units)</param>
        public void ScreenShake(int magnitude, float duration)
        {
            shakeTimer = 0;
            this.duration = duration;
            shakeQuake =  magnitude;
        }
        
        /// <summary>
        /// Returns a value x between a and b such that x is (t * 100)% between a and b
        /// </summary>
        /// <param name="a">The minimum value</param>
        /// <param name="b">The maximum value (is it inclusive or exclusive?  Someone clarify please)</param>
        /// <param name="t">The percentage of the difference between a and b to add to a.</param>
        /// <returns>x, where x is t*100% between a and b</returns>
        public float Lerp(float a, float b, float t)
        {
            //https://forum.yoyogames.com/index.php?threads/how-exactly-does-lerp-work.17177/
            return (a + ((b - a) * t));
        }
    }
}
