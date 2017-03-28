using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;

namespace The_Bond_of_Stone
{
    public class EntityManager
    {
        public List<Entity> entities;
        List<Entity> garbageEntities;

        Camera camera;

        public EntityManager(Camera camera)
        {
            this.camera = camera;
            entities = new List<Entity>();
        }

        public void Update(GameTime gametime, GameState state)
        {
            garbageEntities = new List<Entity>();
            switch (state)
            {
                case GameState.Playing:

                    //Update enemies
                    if (entities.Count > 0)
                    {
                        foreach (GroundEnemy e in entities.Where(s => s is GroundEnemy))
                        {
                            e.Update(gametime);
                            if (!e.Active)
                            {
                                garbageEntities.Add(e);
                            }
                        }
                        foreach (JumpingEnemy e in entities.Where(s => s is JumpingEnemy))
                        {
                            e.Update(gametime);
                            if (!e.Active)
                            {
                                garbageEntities.Add(e);
                            }
                        }
                        foreach (FlyingEnemy e in entities.Where(s => s is FlyingEnemy))
                        {
                            e.Update(gametime);
                            if (!e.Active)
                            {
                                garbageEntities.Add(e);
                            }
                        }

                        foreach (Entity e in garbageEntities)
                            entities.Remove(e);
                    }

                    break;
                case GameState.GameOver:

                    //Update enemies
                    if (entities.Count > 0)
                    {
                        foreach (GroundEnemy e in entities.Where(s => s is GroundEnemy))
                        {
                            e.Update(gametime);
                            if (!e.Active)
                            {
                                garbageEntities.Add(e);
                            }
                        }
                        foreach (JumpingEnemy e in entities.Where(s => s is JumpingEnemy))
                        {
                            e.Update(gametime);
                            if (!e.Active)
                            {
                                garbageEntities.Add(e);
                            }
                        }
                        foreach (FlyingEnemy e in entities.Where(s => s is FlyingEnemy))
                        {
                            e.Update(gametime);
                            if (!e.Active)
                            {
                                garbageEntities.Add(e);
                            }
                        }

                        foreach (Entity e in garbageEntities)
                            entities.Remove(e);
                    }
                    break;
                case GameState.Pause:

                    break;
            }
        }

        public void Draw(SpriteBatch sb, GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                    //Draw enemies
                    if (entities.Count > 0)
                    {
                        foreach (Entity g in entities)
                            g.Draw(sb, Color.White);
                    }
                    break;
                case GameState.GameOver:
                    //Draw enemies
                    if (entities.Count > 0)
                    {
                        foreach (Entity g in entities)
                            g.Draw(sb, Color.White);
                    }
                    break;
                case GameState.Pause:
                    //Draw enemies
                    if (entities.Count > 0)
                    {
                        foreach (Entity g in entities)
                            g.Draw(sb, Color.White);
                    }
                    break;
            }
        }
    }
}
