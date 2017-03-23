using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    public class PlayerStats {
        public Player Player;

        public int Health;
        public int MaxHealth;
        //How long invulnerability lasts after getting hit
        public float graceTime = 1f;
        public float invulnerabilityTimer;
        public bool invulnerable;

        public int Score = 0;
		public int ScoreMultiTicks = 0;
		public int ScoreMultiplier = 1;

        float distance;
        public float Distance {
            get { return distance / Game1.TILE_SIZE; }
        }
        float lastDistance;

        public float Time = 0f;

        bool isAlive = true;
        public bool IsAlive { get { return isAlive; } }

        public PlayerStats(Player player, int maxHealth) {
            Player = player;

            //set the max health (it has to be a multiple of 2 for drawing).
            if (maxHealth % 2 == 1)
                MaxHealth = maxHealth + 1;
            else
                MaxHealth = maxHealth;

            Health = MaxHealth;
        }

        public void Update(GameTime gameTime) {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Calculate scoring, time, and distance
            if (Player.Rect.Left > distance) {
                Score += (int)((distance - lastDistance) * ScoreMultiplier);

                lastDistance = distance;
                distance = Player.Rect.Left;
            }

            Time += elapsed;

            if(invulnerable)
            {
                invulnerabilityTimer += elapsed;
                if (invulnerabilityTimer > graceTime)
                {
                    invulnerable = false;
                    invulnerabilityTimer = 0f;
                }
            }

			if (ScoreMultiTicks >= 4 && ScoreMultiplier < 8) {
				// Hard code here
			}

            //Calculate whether the player died this update
            //if the player is alive
            //and the player has a chunk and is a certain distance below that chunk
            //or if the player is off the screen on the left
            //kill the player.
            if (isAlive &&
                (Player.CurrentChunk != null && Player.Position.Y > Player.CurrentChunk.Bottom + Game1.CHUNK_LOWER_BOUND) ||
                Player.Position.X <= Game1.Camera.Rect.Left) {
                Die();
            }
                
        }

        //removes health from the player. If the health is 0, kills the player.
        public void TakeDamage(int damage) {
            if(!invulnerable)
            {
                Health = MathHelper.Clamp(Health - damage, 0, MaxHealth);
                invulnerable = true;
                invulnerabilityTimer = 0f;
            }

            if (Health == 0)
                Die();
        }

        //Take damage from a source and get knockbacked away from the source
        public void TakeDamage(int damage, Entity e)
        {
            Vector2 knockback;
            if (!invulnerable)
            {
                Health = MathHelper.Clamp(Health - damage, 0, MaxHealth);
                knockback = Player.Position - e.Position;
                Player.KnockBack(knockback * 200);
                invulnerable = true;
                invulnerabilityTimer = 0f;
            }

            if (Health == 0)
                Die();
        }

        public void TickScore() {
			ScoreMultiTicks++;
		}
		//Just sets isAlive to false.
		public void Die() {
            isAlive = false;
        }

        //Resets this object to its default values.
        public void Reset() {
            Score = 0;
            ScoreMultiplier = 1;
            distance = 0;
            lastDistance = distance;
            Time = 0;

            Health = MaxHealth;
            isAlive = true;
        }
    }
}