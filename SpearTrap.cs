﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    class SpearTrap : Entity
    {
        float rotation;

        bool attack = false;
        bool retract = false;
        bool wait = false;
        float attackTimer;
        float retractTimer;
        float waitTimer;

        Vector2 startPosition;
        Vector2 endPosition;
        Vector2 direction;
        Vector2 origin;

        Texture2D trap;
        Rectangle trapRect;

        public new Rectangle Rect
        {
            get
            {
                int x = (int)(Position.X + (texture.Width * 0.5f * Game1.PIXEL_SCALE)) - Game1.hitBox.Width / 2;
                int y = (int)(Position.Y);
                Rectangle hitRect = new Rectangle(x, y, Game1.hitBox.Width, Game1.hitBox.Height);
                return hitRect.RotateRect(rotation, origin);
            }
        }

        public Rectangle drawRect
        {
            get
            {
                Rectangle drawRect = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                Texture.Width * Game1.PIXEL_SCALE,
                Texture.Height * Game1.PIXEL_SCALE
                );

                drawRect.X += drawRect.Width / 2;
                drawRect.Y += drawRect.Height / 2;

                return drawRect;
            }
        }

        public SpearTrap(Vector2 position, Vector2 direction) : base (Graphics.Spear, position)
        {
            Position = new Vector2(position.X + Game1.TILE_SIZE / 2, position.Y + Game1.TILE_SIZE / 2);
            Position = new Vector2(Position.X - (direction.X * Game1.TILE_SIZE), Position.Y - (direction.Y * Game1.TILE_SIZE));
            this.direction = direction;
            startPosition = Position;
            endPosition = startPosition + (direction * texture.Height * 3.5f);

            //Calculate spear rotation
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            rotation += MathHelper.ToRadians(90);

            //Get rect and rotation for trap texture
            if (MathHelper.ToDegrees(rotation) == 180 || MathHelper.ToDegrees(rotation) == 360)
            {
                trap = Graphics.SpearTrap[0];
            }
            else
            {
                trap = Graphics.SpearTrap[1];
            }

            trapRect = new Rectangle((int)startPosition.X, (int)position.Y + Game1.TILE_SIZE, trap.Width, trap.Height);
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            origin = new Vector2(drawRect.X + drawRect.Width / 2, drawRect.Y + drawRect.Height / 2);

            bool ready = !attack && !retract && !wait;

            if (attack)
            {
                Position = Position.TimedMove(endPosition, attackTimer / 0.2f);
                attackTimer += elapsed;
            }
            else if (retract)
            {
                Position = Position.TimedMove(startPosition, retractTimer / 1.9f);
                retractTimer += elapsed;
            }
            else if (wait)
            {
                waitTimer += elapsed;
            }

            if (attackTimer > 0.21f)
            {
                attack = false;
                wait = true;
                attackTimer = 0;
                waitTimer = 0;
            }

            if(waitTimer > 0.5f)
            {
                wait = false;
                waitTimer = 0;
                retract = true;
                retractTimer = 0;
            }

            if(retractTimer > 2f)
            {
                retract = false;
                retractTimer = 0;
                attackTimer = 0;
            }

            //Shoot timer
            if(Vector2.DistanceSquared(Game1.PlayerStats.Player.Position, Position) < 5000 && ready)
            {
                attack = true;
                NotifyNearby();
                attackTimer = 0;
            }
           
        }

        public override void Draw(SpriteBatch spriteBatch, Color color, int depth = 0)
        {
            Rectangle r = new Rectangle(trapRect.X, trapRect.Y, trapRect.Width * Game1.PIXEL_SCALE, trapRect.Height * Game1.PIXEL_SCALE);

            spriteBatch.Draw(trap, destinationRectangle: r, color: color, origin: new Vector2(trap.Width/2, trap.Height/2), rotation: rotation);
        }

        public void DrawSpear(SpriteBatch spriteBatch, Color color, int depth = 0)
        {
            if (Active)
            {
                spriteBatch.Draw(texture: Texture, destinationRectangle: drawRect, color: color, origin: new Vector2(texture.Width / 2, texture.Height / 2), rotation: rotation);
            }
            spriteBatch.Draw(Graphics.DebugTexture, destinationRectangle: Rect, color: Color.Red);
            spriteBatch.Draw(Graphics.BlackTexture, position: origin, color: Color.Black);
        }

        public void NotifyNearby()
        {
            foreach(Entity e in Game1.Player.CurrentChunk.Traps)
            {
                if(e is SpearTrap && Vector2.DistanceSquared(Position, e.Position) < 50000)
                {
                    ((SpearTrap)e).attack = true;
                }
            }
        }
    }
}
