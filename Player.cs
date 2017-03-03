using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
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
    public enum Movement
    {
        Left,
        Right,
        Stop
    }

    public class Player
    {
		public PlayerStats p;

		public PhysicsObject physicsRect;
        PhysicsObject separationRect;
        Texture2D texture;

        float speed = 8.0f;

        bool hitGround;
        bool hitLeft;
        bool hitRight;

        float textureSize;

        public bool Grounded { get; set; }
        public bool Walled { get; set; }


        public Player(World world, Texture2D texture, Vector2 size, float mass, Vector2 startPosition)
        {
            // Create the physics objects of the player
            textureSize = size.X;
            physicsRect = new PhysicsObject(size.X - 2 * size.X / Game1.PixelScaleFactor, size.Y - 2 *size.Y / Game1.PixelScaleFactor, mass / 2f, "player", BodyType.Dynamic);
            //physicsRect = new PhysicsObject(size, 1f, "player", BodyType.Dynamic);
            separationRect = new PhysicsObject(new Vector2(size.X + 1 * UnitConvert.pixelToUnit, size.Y + UnitConvert.pixelToUnit), mass / 2f, "player", BodyType.Dynamic);
            physicsRect.Position = startPosition;
            separationRect.Body.FixedRotation = true;
            physicsRect.Body.FixedRotation = true;

            this.texture = texture;

            physicsRect.Body.OnCollision += Body_OnCollision;
            separationRect.Body.OnSeparation += Body_OnSeparation;
			p = new PlayerStats();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destination = new Rectangle
            (
                (int)physicsRect.Position.X,
                (int)physicsRect.Position.Y,
                (int)textureSize,
                (int)textureSize
            );

            spriteBatch.Draw(texture, destination, null, Color.White, physicsRect.Body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }

        public void Update(GameTime gameTime, World w)
        {
            if (Game1.keyboardState.IsKeyDown(Keys.Left) || Game1.keyboardState.IsKeyDown(Keys.A))
            {
                Move(Movement.Left);
            }
            else if (Game1.keyboardState.IsKeyDown(Keys.Right) || Game1.keyboardState.IsKeyDown(Keys.D))
            {
                Move(Movement.Right);
            }
            else
            {
                Move(Movement.Stop);
            }

            if (Game1.keyboardState.IsKeyDown(Keys.Space) && !Game1.prevKeyboardState.IsKeyDown(Keys.Space))
            {
                Jump(-15f);
            }

            separationRect.Position = physicsRect.Position;

            Walled = hitLeft || hitRight;
            Grounded = hitGround;

            if (Walled && physicsRect.Body.LinearVelocity.Y > 0)
                physicsRect.Body.GravityScale = 0.5f;
            else
                physicsRect.Body.GravityScale = 1.0f;

        }

        public void Move(Movement movement)
        {

            switch (movement)
            {

                case Movement.Left:
                    physicsRect.Body.LinearVelocity = new Vector2(-speed, physicsRect.Body.LinearVelocity.Y);
                    break;

                case Movement.Right:
                    physicsRect.Body.LinearVelocity = new Vector2(speed, physicsRect.Body.LinearVelocity.Y);
                    break;

                case Movement.Stop:
                    physicsRect.Body.LinearVelocity = new Vector2(0, physicsRect.Body.LinearVelocity.Y);
                    break;
            }
        }

        public void Jump(float force = -1f)
        {
            if (Grounded)
            {
                physicsRect.Body.LinearVelocity = new Vector2(physicsRect.Body.LinearVelocity.X, force);

                Grounded = false;
            }
            else
            {
                if (Walled)
                    physicsRect.Body.LinearVelocity = new Vector2(physicsRect.Body.LinearVelocity.X, force);
            }
        }

        public void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            

            if (fixtureB.Body.UserData.ToString() != "ground")
                return;

            if (fixtureA.Body.Position.Y < fixtureB.Body.Position.Y)
                hitGround = false;

            if (fixtureA.Body.Position.X > fixtureB.Body.Position.X)
                hitLeft = false;

             if (fixtureA.Body.Position.X < fixtureB.Body.Position.X)
                hitRight = false;
        }

        public bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //This body collided with a ground physicsobject
            if (fixtureB.Body.UserData.Equals("ground"))
            {
                switch (ContactDirection.Direction(contact))
                {
                    case CollisionDirection.Bottom:
                        hitGround = true;
                        break;

                    case CollisionDirection.Left:
                        hitLeft = true;
                        break;

                    case CollisionDirection.Right:
                        hitRight = true;
                        break;

                    case CollisionDirection.Top:

                        break;
                }
            }else if (fixtureB.Body.UserData.Equals("player"))
                return false;

            return true;
        }
    }
}
