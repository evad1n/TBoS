using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace The_Bond_of_Stone {
    /// <summary>
    /// Camera is a library-free implementation of MonogameExtended's Camera2D.
    /// </summary>
    public class Camera : Camera2D {
        public Rectangle Rect { get; set; }
        Vector2 startPos;

        //Game-specific memebers
        public float Speed { get; set; }
        public Entity Target { get; set; }
        float smoothing = 0.3f;

        //Screen Shake members
        float shakeTimer;
        float duration;
        float shakeQuake;
        

        //Default: doesn't move and has no target.
        public Camera(GraphicsDevice graphicsDevice) : base(graphicsDevice) {
            Rect = new Rectangle((int)(Origin.X - Game1.ScreenWidth / 2), ((int)Origin.Y - Game1.ScreenHeight / 2), Game1.ScreenWidth, Game1.ScreenHeight);

            startPos = new Vector2(Rect.X, Rect.Y);

            Speed = 0f;
            Target = null;
        }
        
        //Has a reference to an entity to "follow" on Y, doesn't move on X.
        public Camera(GraphicsDevice graphicsDevice, Entity target) : base(graphicsDevice) {
            Rect = new Rectangle((int)(Origin.X - Game1.ScreenWidth / 2), ((int)Origin.Y - Game1.ScreenHeight / 2), Game1.ScreenWidth, Game1.ScreenHeight);

            startPos = new Vector2(Rect.X, Rect.Y);

            Speed = 0f;
            Target = target;
        }

        //Has a reference to a target entity and X speed.
        public Camera(GraphicsDevice graphicsDevice, Entity target, float speed) : base(graphicsDevice) {
            Rect = new Rectangle((int)(Origin.X - Game1.ScreenWidth / 2), ((int)Origin.Y - Game1.ScreenHeight / 2), Game1.ScreenWidth, Game1.ScreenHeight);

            startPos = new Vector2(Rect.X, Rect.Y);

            Speed = speed;
            Target = target;
        }

        public void Update(GameTime gameTime) {
            //Entity follow code.
            if(Target != null)
                Origin = new Vector2(Origin.X + Speed, MathHelper.Lerp(Origin.Y, Target.Rect.Y, (float)gameTime.ElapsedGameTime.TotalSeconds / smoothing));
            Rect = new Rectangle((int)(Origin.X - Game1.ScreenWidth / 2), ((int)Origin.Y - Game1.ScreenHeight / 2), Game1.ScreenWidth, Game1.ScreenHeight);

            LookAt(Origin);

            //Screen shake
            if (shakeTimer < duration) {
                shakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                //Change the screen's position by either -shakeQuake or +shakeQuake each frame on each axis, and dampen shakeQuake.
                Position = new Vector2(Position.X + Game1.RandomObject.Next(-1, 2) * shakeQuake, Position.Y + Game1.RandomObject.Next(-1, 2) * shakeQuake);
                shakeQuake = MathHelper.Lerp(shakeQuake, 0, shakeTimer / duration);
            }
        }

        /// <summary>
        /// Shakes the screen from the default point at a given magnitude for a duration.
        /// </summary>
        /// <param name="magnitude">The initial magnitude of the quake.</param>
        /// <param name="duration">The duration of the quake in seconds.</param>
        public void ScreenShake(float magnitude, float duration) {
            shakeTimer = 0;
            this.duration = duration;
            shakeQuake = magnitude;
        }

        public void Reset() {
            Origin = Target.Position;
            LookAt(Origin);
            Rect = new Rectangle((int)(Origin.X - Game1.ScreenWidth / 2), ((int)Origin.Y - Game1.ScreenHeight / 2), Game1.ScreenWidth, Game1.ScreenHeight);
        }
    }
}