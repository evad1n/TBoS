using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    public enum Projectile { Sawblade, Spear, Arrow };

    public class TurretEnemy : Enemy
    {
        float shootTimer;
        float attackSpeed;
        Projectile type;
        Vector2 direction;
        float rotation;

        Player player;
        int yOffset;

        //Animation?
        SpriteEffects facing = SpriteEffects.None;

        public new Rectangle Rect
        {
            get
            {
                return new Rectangle(
                    (int)Math.Round(Position.X / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                    (int)Math.Round(Position.Y / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                    texture.Width * Game1.PIXEL_SCALE,
                    texture.Height * Game1.PIXEL_SCALE
                    );
            }
        }

        public TurretEnemy(Vector2 position, Projectile type, Vector2 direction) : base(Graphics.EnemySlugTextures[0], position)
        {
            Texture = texture;
            Position = position;
            player = Game1.PlayerStats.Player;
            this.type = type;
            this.direction = direction;

            rotation = (float)Math.Atan2(direction.Y, direction.X);

            //Set attack rate
            switch (type)
            {
                case Projectile.Sawblade:
                    attackSpeed = 0.4f;

                    if (direction.X > 0)
                    {
                        facing = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        facing = SpriteEffects.None;
                    }

                    //idk
                    Position = new Vector2(Position.X - 3, Position.Y);
                    texture = Graphics.SawbladeTrap;
                    break;
                case Projectile.Arrow:
                    attackSpeed = 1.5f;
                    
                    if(direction.X != 0) {
                        if (direction.X > 0)
                        {
                            facing = SpriteEffects.FlipHorizontally;
                        }
                        else
                        {
                            facing = SpriteEffects.None;
                        }
                        texture = Graphics.ArrowTrap[0];
                    } else if (direction.Y != 0) {
                        if (direction.Y > 0)
                        {
                            facing = SpriteEffects.FlipVertically;
                        }
                        else
                        {
                            facing = SpriteEffects.None;
                        }
                        texture = Graphics.ArrowTrap[1];
                    }
                    break;
            }
        }

        /// <summary>
        /// Updates player collision and input states.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="keyboardState">Provides a snapshot of inputs.</param>
        /// <param name="prevKeyboardState">Provides a snapshot of the previous frame's inputs.</param>
        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Shoot timer
            shootTimer += elapsed;
            if(shootTimer > 1f / attackSpeed)
            {
                this.Shoot(type, direction);
                shootTimer = 0;
            }

            base.Update(gameTime);
        }


        //This is necessary for altering the player's hitbox. This method lops off the bottom pixel from the hitbox.
        public override void Draw(SpriteBatch spriteBatch, Color color, int depth = 0)
        {
            spriteBatch.Draw(Texture, destinationRectangle: Rect, color: color, effects: facing);
        }
    }
}
