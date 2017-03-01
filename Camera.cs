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
        float shakeTimer;
        float duration; //What does this do?
        float shakeQuake;
        float lerpSpeed;
        bool step = false; //What does this do?
        bool screenShake = false; //It might be best to explain *how* this does what it does.

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

            if(screenShake)
            {
                shakeTimer += 0.05f;
                if (step)
                {
                    Rotate(shakeQuake);
                    step = false;
                }
                else
                {
                    Rotate(-shakeQuake);
                    step = true;
                }

                shakeQuake = Lerp(shakeQuake, 0, shakeTimer / duration);

            }
        }

        public void ScreenShake(int magnitude, float duration)
        {
            shakeTimer = 0;
            this.duration = duration;
            screenShake = true;
            shakeQuake = 0.0005f * (float)magnitude;
        }

        public float Lerp(float a, float b, float speed)
        {
            //https://forum.yoyogames.com/index.php?threads/how-exactly-does-lerp-work.17177/
            return (a + ((b - a) * speed));
        }
    }
}
