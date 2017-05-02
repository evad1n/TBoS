using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    /// <summary>
    /// Wow I learned that from Steve!
    /// Helper stuff by Will
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Rotates a rectangle about a point
        /// </summary>
        /// <param name="rect">The rectangle to be rotated</param>
        /// <param name="rotation">The rotation amount in radians</param>
        /// <param name="origin">The point to rotate around</param>
        /// <returns></returns>
        public static Rectangle RotateRect(this Rectangle rect, float rotation, Vector2 origin)
        {
            rotation = MathHelper.ToRadians(360) - rotation;
            Rectangle result;

            Vector2 topLeft = new Vector2(rect.X, rect.Y);
            Vector2 topRight = new Vector2(rect.X + rect.Width, rect.Y);
            Vector2 botLeft = new Vector2(rect.X, rect.Y + rect.Height);
            Vector2 botRight = new Vector2(rect.X + rect.Width, rect.Y + rect.Height);

            Matrix TranslateTo = Matrix.CreateTranslation(new Vector3(origin.X, origin.Y, 0));
            Matrix TranslateBack = Matrix.CreateTranslation(new Vector3(-origin.X, -origin.Y, 0));
            Matrix rotate = Matrix.CreateRotationZ(rotation);
            Matrix switchY = Matrix.CreateScale(1, -1, 0);

            topLeft = Vector2.Transform(topLeft, TranslateBack);
            topLeft = Vector2.Transform(topLeft, switchY);
            topLeft = Vector2.Transform(topLeft, rotate);
            topLeft = Vector2.Transform(topLeft, switchY);
            topLeft = Vector2.Transform(topLeft, TranslateTo);

            topRight = Vector2.Transform(topRight, TranslateBack);
            topRight = Vector2.Transform(topRight, switchY);
            topRight = Vector2.Transform(topRight, rotate);
            topRight = Vector2.Transform(topRight, switchY);
            topRight = Vector2.Transform(topRight, TranslateTo);

            botLeft = Vector2.Transform(botLeft, TranslateBack);
            botLeft = Vector2.Transform(botLeft, switchY);
            botLeft = Vector2.Transform(botLeft, rotate);
            botLeft = Vector2.Transform(botLeft, switchY);
            botLeft = Vector2.Transform(botLeft, TranslateTo);

            botRight = Vector2.Transform(botRight, TranslateBack);
            botRight = Vector2.Transform(botRight, switchY);
            botRight = Vector2.Transform(botRight, rotate);
            botRight = Vector2.Transform(botRight, switchY);
            botRight = Vector2.Transform(botRight, TranslateTo);

            float left = Math.Min(Math.Min(topLeft.X, topRight.X), Math.Min(botLeft.X, botRight.X));
            float top = Math.Min(Math.Min(topLeft.Y, topRight.Y), Math.Min(botLeft.Y, botRight.Y));

            result = new Rectangle((int)Math.Round(left), (int)Math.Round(top), rect.Width, rect.Height);

            return result;
        }

        /// <summary>
        /// Returns a vector that travels to target at the specificed speed (set the velocity to this vector)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="target"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static Vector2 Move(this Vector2 start, Vector2 target, float speed)
        {
            Vector2 v = target - start;
            if (v.Length() != 0)
            {
                v.Normalize();
            }
            return v * speed;
        }

        /// <summary>
        /// Lerp from this position to the target.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="target"></param>
        /// <param name="amount">How far along to the target you are</param>
        /// <returns></returns>
        public static Vector2 TimedMove(this Vector2 start, Vector2 target, float amount)
        {
            float x = start.X + ((target.X - start.X) * amount);
            float y = start.Y + ((target.Y - start.Y) * amount);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Instantiates a bullet
        /// </summary>
        public static void Shoot(this Enemy e, Projectile type, Vector2 direction)
        {
            Vector2 shootPos = new Vector2(e.Position.X, e.Position.Y);
            switch (type)
            {
                case Projectile.Sawblade:
                    Game1.Entities.projectiles.Add(new Bullet(direction, 200, Graphics.Sawblade, shootPos, 5, type, true, 0));
                    break;
                case Projectile.Spear:
                    Game1.Entities.projectiles.Add(new Bullet(direction, 700, Graphics.Spear, shootPos, 0, Projectile.Spear, false, 20, e));
                    break;
                case Projectile.Arrow:
                    Game1.Entities.projectiles.Add(new Bullet(direction, 600, Graphics.Arrow, shootPos, 0, type, false, 0));
                    break;
            }
        }
    }
}
