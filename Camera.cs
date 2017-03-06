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
        //The actual viewing rectangle bounds of the camera
        public Rectangle rect { get; set; }
        public GraphicsDevice graphicsDevice { get; set; }
        //The left-translation-speed of the Camera.
        float speed = 0.5f;

        private Vector2 startPos;

        //The smoothing factor of the camera's follow behavior (greater values = slower following)
        float smoothing = 0.3f;

        //Should the camera snap to a grid (makes it look UGLY)
        public bool Snapping { get; set; }

        //ScreenShake variables
        //How long screenshake has been going
        float shakeTimer;
        //The total time screenshake should be active for
        float duration;
        //The magnitude of the shaking
        float shakeQuake;
        //How much the screen rotates by
        float rotation;
        //What direction (CCW or CC)
        int direction = 1;
        //The number of times the screen has rotated in the same direction
        int count = 0;
        //Should screenshake rotate screen or just shake
        bool rotating = true;

        public Camera(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Snapping = false;
            rect = new Rectangle((int)(Origin.X - Game1.screenWidth / 2), ((int)Origin.Y - Game1.screenHeight / 2), Game1.screenWidth, Game1.screenHeight);
            startPos = Origin;
        } 

        public void Update(GameTime gameTime)
        {
            //Player follow code.  //Note:  The camera should probably follow the player's y directly till the end of the lowest currently generated chunk, and then stop.  Otherwise it might kill the player when they get a run of really large negative y deviations.
            Origin = new Vector2(Origin.X + speed, Lerp(Origin.Y, target.physicsRect.Position.Y, (float)gameTime.ElapsedGameTime.TotalSeconds / smoothing));
            rect = new Rectangle((int)(Origin.X - Game1.screenWidth / 2), ((int)Origin.Y - Game1.screenHeight / 2), Game1.screenWidth, Game1.screenHeight);

            if(Snapping)
            {
                LookAt(new Vector2(Snap(Origin.X), Snap(Origin.Y)));
            }
            else
            {
                LookAt(Origin);
            }

            //Screen shake code
            if (shakeTimer < duration)
            {
                shakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (rotating)
                {
                    //Switch direction after 2 ticks
                    if (count % 2 == 0)
                    {
                        direction *= -1;
                    }
                    //Make sure that each rotation is the same so the net rotation is 0 (as close to 0 as possible whatever it gets fixed later)
                    if (count > 3)
                    {
                        direction *= -1;
                        rotation = Lerp(shakeQuake/1000, 0, shakeTimer / duration);
                        count = 0;
                    }

                    Rotate(rotation * direction);
                    count++;
                }
                else
                {
                    //Change the screen's position by either -shakeQuake or +shakeQuake each frame on each axis, and dampen shakeQuake.
                    Position = new Vector2(Position.X + Game1.RandomObject.Next(-1, 2) * rotation, Position.Y + Game1.RandomObject.Next(-1, 2) * rotation);
                    rotation = Lerp(shakeQuake, 0, shakeTimer / duration);
                }
            }
            else
            {
                //Make sure rotation is 0...this actually doesnt look bad at all thanks to the partial fix above
                Rotation = 0;
            }
        }


        /// <summary>
        /// Shakes the screen from the default point at a given magnitude for a given duration.
        /// </summary>
        /// <param name="magnitude">The initial magnitude of the quake in (units)</param>
        /// <param name="duration">The duration of the quake in (units)</param>
        public void ScreenShake(int magnitude, float duration, bool rotating)
        {
            this.rotating = rotating;
            shakeTimer = 0;
            this.duration = duration;
            shakeQuake = magnitude;
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

        //Snap camera movement to pixel grid for.....consistency
        public float Snap(float a)
        {
            //Divide by 8 because that is the size of a tile
            float scale = Game1.PixelScaleFactor / 8;
            int rounded = (int)(a / scale);
            return (rounded * scale);
        }

        //For debugging or something idk
        public void Draw(SpriteBatch sb, Color color)
        {
            //sb.Draw(Game1.foregroundTiles[0], rect, color);
        }

        public void Reset()
        {
            Origin = startPos;
        }
    }
}
