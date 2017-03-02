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
        //The speed of the Camera.
        float speed = 0.5f;

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
        //Determines if screenshake should be running
        bool screenShake = false;

        public Camera(GraphicsDevice graphicsDevice, Player target) : base(graphicsDevice)
        {
            this.target = target;            
        } 

        public void Update(GameTime gameTime)
        {
            LookAt(new Vector2(Origin.X, target.physicsRect.Position.Y));
            Origin = new Vector2(Origin.X + speed, Origin.Y);

            if(shakeTimer > duration)
            {
                screenShake = false;
            }

            if (screenShake)
            {
                shakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                //Switch direction after 2 ticks
                if (count % 2 == 0)
                {
                    direction *= -1;
                }
                //Make sure that each rotation is even so the net rotation is 0 (as close to 0 as possible whatever it gets fixed later)
                if (count > 3)
                {
                    direction *= -1;
                    rotation = Lerp(shakeQuake, 0, shakeTimer / duration);
                    count = 0;
                }

                Rotate(rotation * direction);
                count++;
            }
            else
            {
                //Make sure rotation is 0...
                Rotation = 0;
            }
        }

        public void ScreenShake(int magnitude, float duration)
        {
            shakeTimer = 0;
            this.duration = duration;
            screenShake = true;

            //Convert magnitude scale to usable numbers
            shakeQuake = 0.005f * (float)magnitude;
        }

        public float Lerp(float a, float b, float speed)
        {
            //https://forum.yoyogames.com/index.php?threads/how-exactly-does-lerp-work.17177/
            return (a + ((b - a) * speed));
        }
    }
}
