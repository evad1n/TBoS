using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    public class Enemy : Entity
    {

        public Vector2 velocity;

        public Enemy(Texture2D texture, Vector2 position) : base(texture, position)
        {
            Texture = texture;
            Position = position;
        }

        /// <summary>
        /// Updates player collision and input states.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="keyboardState">Provides a snapshot of inputs.</param>
        /// <param name="prevKeyboardState">Provides a snapshot of the previous frame's inputs.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (CurrentChunk != null && (Position.X + Rect.Width < Game1.Camera.Rect.Left || Position.Y > CurrentChunk.Bottom + Game1.CHUNK_LOWER_BOUND))
            {
                Active = false;
            }
        }

        /// <summary>
        /// checks whether the player is "next to" a collidable surface.
        /// </summary>
        /// <param name="offset">The direction of the check.</param>
        /// <returns>Boolean is true when the player's rect offset by "offset" is colliding with the level.</returns>
        public bool CheckCardinalCollision(Vector2 offset)
        {
            if (CurrentChunk != null)
            {
                Rectangle check = Rect;
                check.Offset(offset);
                return CollisionHelper.IsCollidingWithChunk(CurrentChunk, check);
            }
            else
                return false;
        }

        public void KnockBack(Vector2 boom)
        {
            velocity.X = boom.X;
            velocity.Y = boom.Y;
            Game1.Camera.ScreenShake(4f, 0.3f);
        }

        public virtual void Kill()
        {
            Sound.EnemyDeath.Play();

            int particlesToMake = Game1.RandomObject.Next(3, 6);

            for(int i = 0; i < particlesToMake; i++)
            {
                Game1.Entities.particles.Add(new DynamicParticle(Graphics.EnemyParticles[0], Graphics.EnemyParticles, new Vector2(Rect.Center.X, Rect.Center.Y), 5f, new Vector2(200, Game1.RandomObject.Next(-750, -200))));
            }

            Active = false;
        }
    }
}
