using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    public class ArrowLauncher : Entity
    {
        bool facingRight;

        float maxLaunchWaitTime;
        float timer;

        public ArrowLauncher(Texture2D texture, Vector2 position, float launchSpeed, bool facingRight) : base(texture, position, true, true) {
            Texture = texture;
            Position = position;

            this.facingRight = facingRight;
            maxLaunchWaitTime = launchSpeed;
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(timer < maxLaunchWaitTime)
            {
                timer += elapsed;
                if(timer >= maxLaunchWaitTime)
                {
                    LaunchAnArrow();
                    timer = 0;
                }
            }
        }

        public void LaunchAnArrow() {
            //Create a dynamic arrow projectile which follows a straight path according to Direction
            if (facingRight)
            {
                //Create a right-traveling arrow
            }
            else
            {
                //Create a left-traveling arrow
            }
        }
    }
}
