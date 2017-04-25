using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone
{
    class Sound
    {
        ContentManager Content;

        public static SoundEffect EnemyDeath;
        public static SoundEffect MultiplierIncrease;
        public static SoundEffect PickupCoin;
        public static SoundEffect PickupGem;
        public static SoundEffect PlayerDeath;
        public static SoundEffect PlayerJump;
        public static SoundEffect PlayerLandHard;
        public static SoundEffect PlayerLandSoft;
        public static SoundEffect PlayerTakeDamage;
        public static SoundEffect PlayerWallJump;
        public static SoundEffect PlayerWallSlide;
        public static SoundEffect PlayerWalk;

        public static SoundEffect ButtonClick;

        public static Song MusicTrack;

        public void LoadContent(ContentManager Content)
        {
            this.Content = Content;

            EnemyDeath = Load("enemyDeath");
            MultiplierIncrease = Load("multiplierIncrease");
            PickupCoin = Load("pickupCoin");
            PickupGem = Load("pickupGem");
            PlayerDeath = Load("playerDeath");
            PlayerJump = Load("playerJump");
            PlayerLandHard = Load("playerLandHard");
            PlayerLandSoft = Load("playerLandSoft");
            PlayerTakeDamage = Load("playerTakeDamage");
            PlayerWallJump = Load("playerWalljump");
            PlayerWallSlide = Load("playerWallslide");
            PlayerWalk = Load("playerWalk");

            ButtonClick = Load("buttonClick");

            MusicTrack = Content.Load<Song>(@"audio\music");
        }

        SoundEffect Load(string name) {
            return Content.Load<SoundEffect>(@"audio\" + name);
        }
    }
}
