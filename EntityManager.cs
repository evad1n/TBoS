using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;

namespace The_Bond_of_Stone
{
    public class EntityManager
    {
        public List<Enemy> enemies;
        public List<Bullet> projectiles;
        List<Entity> garbageEntities;
        List<Bullet> garbageProjectiles;

        Camera camera;

        public EntityManager(Camera camera)
        {
            this.camera = camera;
            enemies = new List<Enemy>();
            projectiles = new List<Bullet>();
        }

        public void Update(GameTime gametime, GameState state)
        {
            garbageEntities = new List<Entity>();
            garbageProjectiles = new List<Bullet>();

            switch (state)
            {
                case GameState.Playing:

                    //Update enemies
                    foreach (Enemy e in enemies)
                    {
                        e.Update(gametime);
                        if (!e.Active)
                        {
                            garbageEntities.Add(e);
                        }
                    }

                    //Update projectiles
                    foreach (Bullet e in projectiles)
                    {
                        e.Update(gametime);
                        if (!e.Active)
                        {
                            garbageProjectiles.Add(e);
                        }
                    }

                    foreach (Enemy e in garbageEntities)
                        enemies.Remove(e);

                    foreach (Bullet e in garbageProjectiles)
                        projectiles.Remove(e);

                    break;
                case GameState.GameOver:

                    //Update enemies
                    foreach (Enemy e in enemies)
                    {
                        e.Update(gametime);
                        if (!e.Active)
                        {
                            garbageEntities.Add(e);
                        }
                    }

                    //Update projectiles
                    foreach (Bullet e in projectiles)
                    {
                        e.Update(gametime);
                        if (!e.Active)
                        {
                            garbageProjectiles.Add(e);
                        }
                    }

                    foreach (Enemy e in garbageEntities)
                        enemies.Remove(e);

                    foreach (Bullet e in garbageProjectiles)
                        projectiles.Remove(e);

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
                    if (enemies.Count > 0)
                    {
                        foreach (Entity g in enemies)
                            g.Draw(sb, Color.White);
                    }

                    //Draw projectiles
                    if (enemies.Count > 0)
                    {
                        foreach (Bullet b in projectiles)
                            b.Draw(sb, Color.White);
                    }
                    break;
                case GameState.GameOver:
                    //Draw enemies
                    if (enemies.Count > 0)
                    {
                        foreach (Entity g in enemies)
                            g.Draw(sb, Color.White);
                    }

                    //Draw projectiles
                    if (enemies.Count > 0)
                    {
                        foreach (Bullet b in projectiles)
                            b.Draw(sb, Color.White);
                    }
                    break;
                case GameState.Pause:
                    //Draw enemies
                    if (enemies.Count > 0)
                    {
                        foreach (Entity g in enemies)
                            g.Draw(sb, Color.White);
                    }

                    //Draw projectiles
                    if (enemies.Count > 0)
                    {
                        foreach (Bullet b in projectiles)
                            b.Draw(sb, Color.White);
                    }
                    break;
            }
        }
    }
}
