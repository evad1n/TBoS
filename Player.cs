
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TheBondOfStone
{
    /// <summary>
    /// The drection of movement.
    /// </summary>
    public enum Movement
    {
        Left,
        Right,
        Stop
    }

    public class Player
    {
		public PlayerStats p;

        Camera camera;
        Texture2D texture;

        KeyboardState prevKbState;

        float speed = 8.0f;

        float textureSize;

        public bool Grounded { get; set; }
        public bool Alive { get; set; }

        public Vector2 Position { get; set; }


        public Player(Texture2D texture, Vector2 size, float mass, Vector2 startPosition, Camera camera)
        {
            textureSize = size.X;

            this.texture = texture;
            //SETS THE CAMERA DOM DONT DELETE IT DONT DO IT PLEASE GOD NO
            this.camera = camera;
            Alive = true;

			p = new PlayerStats();
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(texture, Position, color);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            //Check if player is off screen
            if (Position.X < camera.rect.Left || Position.X > camera.rect.Right || Position.Y > camera.rect.Bottom || Position.Y < camera.rect.Top)
            {
                Alive = false;
            }

            if (kb.IsKeyDown(Keys.W))
            {
                Position = new Vector2(Position.X, Position.Y - speed);
            }
            if (kb.IsKeyDown(Keys.A))
            {
                Position = new Vector2(Position.X - speed, Position.Y);
            }
            if (kb.IsKeyDown(Keys.S))
            {
                Position = new Vector2(Position.X, Position.Y + speed);
            }
            if (kb.IsKeyDown(Keys.D))
            {
                Position = new Vector2(Position.X + speed, Position.Y);
            }

            prevKbState = kb;
        }

    }
}
