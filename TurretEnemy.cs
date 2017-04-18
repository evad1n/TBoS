using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    public enum Projectile { Sawblade, Spear, Arrow, Grenade};

    public class TurretEnemy : Enemy
    {
        float shootTimer;
        float attackSpeed;
        Projectile type;
        Vector2 direction;

        Player player;
        int yOffset;

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

        public TurretEnemy(Vector2 position, Projectile type, Vector2 direction) : base(Graphics.EnemySlugTextures[0], position)
        {
            Texture = texture;
            Position = position;
            player = Game1.PlayerStats.Player;
            this.type = type;
            this.direction = direction;

            //Set attack rate
            switch (type)
            {
                case Projectile.Sawblade:
                    attackSpeed = 0.4f;
                    break;
                case Projectile.Spear:
                    attackSpeed = 0.7f;
                    break;
                case Projectile.Arrow:
                    attackSpeed = 0.8f;
                    break;
                case Projectile.Grenade:
                    attackSpeed = 0.5f;
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
            if(shootTimer > 1f / attackSpeed && Game1.PlayerStats.IsAlive)
            {
                Shoot();
                shootTimer = 0;
            }

            base.Update(gameTime);

            if(direction != null)
            {
                if (player.Position.X < Position.X)
                {
                    facing = SpriteEffects.None;
                }
                else
                {
                    facing = SpriteEffects.FlipHorizontally;
                }
            }
        }


        public void Shoot()
        {

            switch (type)
            {
                case Projectile.Sawblade:
                    Game1.Entities.projectiles.Add(new Bullet(this, direction, 200, Graphics.Sawblade, Position, 5, true, 5));
                    break;
                case Projectile.Spear:
                    Game1.Entities.projectiles.Add(new Bullet(this, direction, 700, Graphics.Spear, Position, 0, false, 20));
                    break;
                case Projectile.Arrow:
                    Game1.Entities.projectiles.Add(new Bullet(this, direction, 800, Graphics.Arrow, Position, 0, false, 20));
                    break;
                case Projectile.Grenade:
                    Game1.Entities.projectiles.Add(new Bullet(this, direction, 300, Graphics.Grenade, Position, 1, true, 15));
                    break;
            }
        }

        //This is necessary for altering the player's hitbox. This method lops off the bottom pixel from the hitbox.
        public override void Draw(SpriteBatch spriteBatch, Color color, int depth = 0)
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
