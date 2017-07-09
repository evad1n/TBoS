using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace The_Bond_of_Stone {
    /// <summary>
    /// Extension of Monogame.Extended's Camera2D. Has extra functionality for following the player and other goodies.
    /// 
    /// By Will Dickinson
    /// </summary>
    public class Camera : Camera2D {
        public Rectangle Rect { get; set; }
        Chunk nextChunk;
        Vector2 gameOverPath;

        float XSpeedUpBound = Game1.TILE_SIZE * 4;

        float smoothingTimer = 0f;

        //Game-specific memebers
        public float initialSpeed;
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

            Speed = 0f;
            Target = null;

            initialSpeed = 0;
        }
        
        //Has a reference to an entity to "follow" on Y, doesn't move on X.
        public Camera(GraphicsDevice graphicsDevice, Entity target) : base(graphicsDevice) {
            Rect = new Rectangle((int)(Origin.X - Game1.ScreenWidth / 2), ((int)Origin.Y - Game1.ScreenHeight / 2), Game1.ScreenWidth, Game1.ScreenHeight);

            Speed = 0f;
            Target = target;

            initialSpeed = 0;
        }

        //Has a reference to a target entity and X speed.
        public Camera(GraphicsDevice graphicsDevice, Entity target, float speed) : base(graphicsDevice) {
            Rect = new Rectangle((int)(Origin.X - Game1.ScreenWidth / 2), ((int)Origin.Y - Game1.ScreenHeight / 2), Game1.ScreenWidth, Game1.ScreenHeight);

            Speed = speed;
            Target = target;

            initialSpeed = speed;
        }

        public void Update(GameTime gameTime) {
            nextChunk = Game1.Generator.GetEntityChunkID(gameOverPath);
            gameOverPath = new Vector2(gameOverPath.X + Speed, gameOverPath.Y);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Player follow code.
            if (Target != null)
            {
                gameOverPath = new Vector2(gameOverPath.X, Origin.Y);
                Origin = new Vector2(Origin.X + Speed, MathHelper.Lerp(Origin.Y, Target.Rect.Y, (float)gameTime.ElapsedGameTime.TotalSeconds / smoothing));
            }
            //Game over pathfinding code
            else
            {
                gameOverPath += Move(gameOverPath, new Vector2(gameOverPath.X, nextChunk.Rect.Top + nextChunk.Rect.Height / 4), Speed);
                Origin = new Vector2(Origin.X + Speed, MathHelper.Lerp(Origin.Y, gameOverPath.Y, (float)gameTime.ElapsedGameTime.TotalSeconds / 0.5f));
            }

            Rect = new Rectangle((int)(Origin.X - Game1.ScreenWidth / 2), ((int)Origin.Y - Game1.ScreenHeight / 2), Game1.ScreenWidth, Game1.ScreenHeight);

            LookAt(Origin);

            if (Target != null && Target.Rect.X + Target.Rect.Width > Rect.Right - XSpeedUpBound)
                Speed = 3f;
            else
                Speed = initialSpeed;

            //Screen shake
            if (shakeTimer < duration)
            {
                shakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                //Change the screen's position by either -shakeQuake or +shakeQuake each frame on each axis, and dampen shakeQuake.
                Position = new Vector2(Position.X + Game1.RandomObject.Next(-1, 2) * shakeQuake, Position.Y + Game1.RandomObject.Next(-1, 2) * shakeQuake);
                shakeQuake = MathHelper.Lerp(shakeQuake, 0, shakeTimer / duration);
            }
            else
                shakeQuake = 0;
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
            gameOverPath = new Vector2(Rect.Right - 50, Origin.Y);
        }

        public Vector2 Move(Vector2 start, Vector2 target, float speed)
        {
            Vector2 v = target - start;
            if(v.Length() != 0)
            {
                v.Normalize();
            }
            return v * speed;
        }

        //For debugging THESE COORDINATES ARE RELATIVE TO CAMERA HAHAHAHAHAHAHAHHAHA
        public void Draw(SpriteBatch sb)
        {
            //sb.Draw(Graphics.DebugTexture, gameOverPath, Color.White);
        }
    }
}