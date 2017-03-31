using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    public class TurretEnemy : Enemy
    {
        float bulletSpeed = 500f;
        float shootTimer;

        Player player;
        int yOffset;
        Texture2D bulletTexture = Graphics.Spike_Right[0];

        //Animation?
        SpriteEffects facing = SpriteEffects.None;

        public new Rectangle Rect
        {
            get
            {
                yOffset = (Game1.TILE_PIXEL_SIZE - Graphics.EnemySlugTextures[0].Height) * 3;

                return new Rectangle(
                    (int)Math.Round(Position.X / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                    (int)Math.Round(Position.Y / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                    Graphics.EnemySlugTextures[0].Width * Game1.PIXEL_SCALE,
                    Graphics.EnemySlugTextures[0].Height * Game1.PIXEL_SCALE
                    );
            }
        }

        public TurretEnemy(Texture2D texture, Vector2 position) : base(texture, position)
        {
            Texture = texture;
            Position = position;
            player = Game1.PlayerStats.Player;
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
            if(shootTimer > 1 && Game1.PlayerStats.IsAlive)
            {
                Shoot();
                shootTimer = 0;
            }

            base.Update(gameTime);

            if (player.Position.X < Position.X)
            {
                facing = SpriteEffects.None;
            }
            else
            {
                facing = SpriteEffects.FlipHorizontally;
            }
        }


        public void Shoot()
        {
            float dist = Vector2.Distance(player.Position, Position);
            Vector2 target = ((dist / bulletSpeed) * player.velocity) + player.Position;

            //Vector2 target = player.Position;
            Game1.Entities.projectiles.Add(new Bullet(this, target, bulletSpeed, bulletTexture, Position));
        }

        //This is necessary for altering the player's hitbox. This method lops off the bottom pixel from the hitbox.
        public override void Draw(SpriteBatch spriteBatch, Color color)
        {

            if (Active)
            {
                if (LockToPixelGrid)
                {
                    Rectangle drawRect = new Rectangle(
                        (int)Math.Round(Position.X / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                        (int)Math.Round((Position.Y + Game1.PIXEL_SCALE) / Game1.PIXEL_SCALE) * Game1.PIXEL_SCALE,
                        Texture.Width * Game1.PIXEL_SCALE,
                        Texture.Height * Game1.PIXEL_SCALE
                        );

                    spriteBatch.Draw(Texture, destinationRectangle: Rect, color: color, effects: facing);
                }
                else
                    spriteBatch.Draw(Texture, destinationRectangle: Rect, color: color, effects: facing);
            }
        }
    }
}
