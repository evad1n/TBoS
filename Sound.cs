using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
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

        public void LoadContent(ContentManager Content)
        {
            this.Content = Content;

            EnemyDeath = Content.Load<SoundEffect>(@"audio\enemyDeath");
            MultiplierIncrease = Content.Load<SoundEffect>(@"audio\multiplierIncrease");
            PickupCoin = Content.Load<SoundEffect>(@"audio\pickupCoin");
            PickupGem = Content.Load<SoundEffect>(@"audio\pickupGem");
            PlayerDeath = Content.Load<SoundEffect>(@"audio\playerDeath");
            PlayerJump = Content.Load<SoundEffect>(@"audio\playerJump");
            PlayerLandHard = Content.Load<SoundEffect>(@"audio\playerLandHard");
            PlayerLandSoft = Content.Load<SoundEffect>(@"audio\playerLandSoft");
            PlayerTakeDamage = Content.Load<SoundEffect>(@"audio\playerTakeDamage");
            PlayerWallJump = Content.Load<SoundEffect>(@"audio\playerWalljump");
            PlayerWallSlide = Content.Load<SoundEffect>(@"audio\playerWallslide");
        }
    }
}
