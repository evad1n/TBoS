using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    public static class CollisionHelper
    {
        /// <summary>
        /// solidIDs array holds the IDs of tiles which the player is not able to move through.
        /// </summary>
        static int[] solidIDs = { 1, 3, 4, 5 };

        /// <summary>
        /// Determines how detailed collision checking will be. higher values = higher precision.
        /// </summary>
        static float collisionCorrectionPrecision = 2f;

        /// <summary>
        /// Returns whether a tile ID can be moved through by the player.
        /// </summary>
        /// <param name="ID">The tile ID to check against.</param>
        /// <returns></returns>
        public static bool IsPassable(int ID) {
            //If this ID isn't in the list of IDs that represent solid tiles, then it is a passable tile.
            if (solidIDs.Contains(ID))
                return false;

            return true;
        }

        /// <summary>
        /// Returns whether the rect toCheck is intersecting with the pseudo-geometry of chunk.
        /// </summary>
        /// <param name="chunk">The chunk whose tiles should be checked.</param>
        /// <param name="toCheck">The rectangle to check collision against.</param>
        /// <returns></returns>
        public static bool IsCollidingWithChunk(Chunk chunk, Rectangle toCheck)
        {
            Rectangle rect;

            //If the rectangle is off the screen on the right, always return true.
            if (toCheck.X + toCheck.Width > Game1.Camera.Rect.Right)
                return true;

            if (chunk != null) {
				//Check each tile in this chunk
				foreach (Tile t in chunk.Tiles) {
					//Save the rectangle of this tile as it should be represented on the collision grid.
					rect = new Rectangle(t.Rect.X - Game1.TILE_SIZE / 2, t.Rect.Y - Game1.TILE_SIZE / 2, t.Rect.Width, t.Rect.Height);

					//If this is a solid tile and it is intersecting with the rectangle to check against, there is a collision.
					if (!IsPassable(t.ID) && rect.Intersects(toCheck))
						return true;
				}
			}

            //Otherwise, there is no collision.
            return false;
        }

        //Note: the following method is sub-optimal. It probably isn't going to be an issue, but a better solution would be to
        //determine the "collision depth" and correct the position vector based on that, rather than try many vectors along a path
        
        /// <summary>
        /// Recursive method to determine the furthest available position for a moving rectangle given an origin and destination.
        /// Recursion is used to separate the components of the movement.
        /// </summary>
        /// <param name="origin">The staring position.</param>
        /// <param name="dest">The desired position.</param>
        /// <param name="bounds">The bounding rectangle of the moving object.</param>
        /// <param name="chunk">The chunk whose pseudo-geometry to check against.</param>
        /// <returns>Unoccupied position vector for rect on the path from origin to dest.</returns>
        public static Vector2 DetailedCollisionCorrection(Vector2 origin, Vector2 dest, Rectangle rect, Chunk chunk)
        {
            //Get the vector between the origin and destination.
            Vector2 attempt = dest - origin;
            //Set the iterable "final position" to the first position we'll check (the origin).
            Vector2 loc = origin;

            //Divide the attempt vector into several segments.
            int steps = (int)(attempt.Length() * collisionCorrectionPrecision + 1);
            //Get the vector represented by one of these steps.
            Vector2 oneStep = attempt / steps;

            //For each step on the path...
            for(int i = 1; i <= steps; i++)
            {
                //Save the vector representing this step's movement "attempt"
                Vector2 att = origin + (oneStep * i);
                //Create a rectangle at the attempt position.
                Rectangle newRect = CreateDummyRectangle(att, rect.Width, rect.Height);
                //If the new rectangle is colliding with the level geometry, this is the furthest movement as represented by the steps.
                if (!IsCollidingWithChunk(chunk, newRect))
                    loc = att;

                //Otherwise, the movement is not valid.
                else {
                    //...But we should check to see if one of the components of the movement is valid.
                    bool separateComponents = attempt.X != 0 && attempt.Y != 0;

                    //If the components of this movement are not along one axis, separate the components.
                    if (separateComponents) {
                        //Save the number of remaining steps on the attempt vector
                        int remainingSteps = steps - (i - 1);

                        //Only one of the following blocks will run when this method is recursed the second time.
                        //X Component-wise movement
                        //Determine the vector of the remaining X movement, and the destination X movement.
                        Vector2 remainingMoveX = oneStep.X * Vector2.UnitX * remainingSteps;
                        Vector2 finalXMove = loc + remainingMoveX;
                        //Perform this method on the remaining X movement and destination. 
                        loc = DetailedCollisionCorrection(loc, finalXMove, rect, chunk);

                        //Y Component-wise movement
                        //Determine the vector of the remaining Y movement, and the destination Y movement.
                        Vector2 remainingMoveY = oneStep.Y * Vector2.UnitY * remainingSteps;
                        Vector2 finalYMove = loc + remainingMoveY;
                        //Perform this method on the remaining Y movement and destination. 
                        loc = DetailedCollisionCorrection(loc, finalYMove, rect, chunk);
                    }
                    break;
                }
            }

            return loc;
        }

        /// <summary>
        /// Returns a rectangle to check collisions against.
        /// </summary>
        /// <param name="position">Position of the rectangle.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <returns></returns>
        static Rectangle CreateDummyRectangle(Vector2 position, int width, int height) {
            return new Rectangle((int)position.X, (int)position.Y, width, height);
        }
    }
}
