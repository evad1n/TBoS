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
    public class Camera : Camera2D
    {
        //The target player of this Camera.
        public Player target { get; set; }

        public GraphicsDevice graphicsDevice { get; set; }
        //The left-translation-speed of the Camera.
        float speed = 0.5f;

        //The smoothing factor of the camera's follow behavior (greater values = slower following)
        float smoothing = 0.3f;

        //Should the camera snap to a grid (makes it look UGLY)
        public bool Snapping { get; set; }

        //Shaking parameters
        float shakeTimer;
        float duration; 
        float shakeQuake;

        public Camera(GraphicsDevice graphicsDevice, Player target) : base(graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this.target = target;
            Snapping = false;
        } 

        public void Update(GameTime gameTime)
        {
            //Player follow code.
            Origin = new Vector2(Origin.X + speed, Lerp(Origin.Y, target.physicsRect.Position.Y, (float)gameTime.ElapsedGameTime.TotalSeconds / smoothing));

            if(Snapping)
            {
                LookAt(new Vector2(Snap(Origin.X), Snap(Origin.Y)));
            }
            else
            {
                LookAt(Origin);
            }

            //Screen shake code
            if (shakeTimer < duration) {
                shakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                //Change the screen's position by either -shakeQuake or +shakeQuake each frame on each axis, and dampen shakeQuake.
                Position = new Vector2(Position.X + Game1.RandomObject.Next(-1, 2) * shakeQuake, Position.Y + Game1.RandomObject.Next(-1, 2) * shakeQuake);
                shakeQuake = Lerp(shakeQuake, 0, shakeTimer / duration);              
            }
        }

        //Gets a shaking intensity and duration, and "shakes" the screen accordingly
        public void ScreenShake(int magnitude, float duration)
        {
            shakeTimer = 0;
            this.duration = duration;
            shakeQuake =  magnitude;
        }

        //Returns a value x between a and b such that x is (t * 100)% between a and b
        public float Lerp(float a, float b, float t)
        {
            //https://forum.yoyogames.com/index.php?threads/how-exactly-does-lerp-work.17177/
            return (a + ((b - a) * t));
        }

        //Snap camera movement to pixel grid for.....consistency
        public float Snap(float a)
        {
            //Divide by 8 because that is the size of a tile
            float scale = Game1.PixelScaleFactor / 8;
            int rounded = (int)(a / scale);
            return (rounded * scale);
        }
    }
}
