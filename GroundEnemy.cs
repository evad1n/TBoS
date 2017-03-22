using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    class GroundEnemy : Entity
    {
        float speed = 1f;
        int direction;

        Chunk nextChunk;
        Rectangle gapRect;
        Rectangle wallRect;

        Vector2 previousPosition;

        public GroundEnemy(Texture2D texture, Vector2 position) : base(texture, position) {
            Texture = texture;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            previousPosition = Position;

            //Update pathfinding colliders
            gapRect = new Rectangle(Rect.X + Game1.TILE_SIZE, Rect.Y + Game1.TILE_SIZE, Game1.TILE_SIZE, Game1.TILE_SIZE);
            wallRect = new Rectangle(Rect.X + Game1.TILE_SIZE, Rect.Y, Game1.TILE_SIZE, Game1.TILE_SIZE);
            nextChunk = Game1.Generator.GetEntityChunkID(gapRect);
            //Check for pathfinding (gaps and walls)
            if (CollisionHelper.IsCollidingWithChunk(nextChunk, gapRect) && !CollisionHelper.IsCollidingWithChunk(nextChunk, wallRect))
            {
                direction *= -1;
            }

            //IDK how to do collisions jesus save me!
            Position = new Vector2(Position.X + (speed * direction), Position.Y);
            if (CurrentChunk != null && Game1.PlayerStats.IsAlive)
                Position = CollisionHelper.DetailedCollisionCorrection(previousPosition, Position, Rect, CurrentChunk);
        }

        //This will do something eventually
        public void Kill()
        {
            Game1.PlayerStats.TickScore();

            Active = false;
        }
    }
}
