using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace The_Bond_of_Stone {
    class Graphics {
        //BASIC
        ContentManager Content;

        //TILE ARRAYS
        public static Texture2D[] Tiles_ground;
        public static Texture2D[] Tiles_gold;
        public static Texture2D[] Tiles_background;

        public static Texture2D[] Deco_groundTop;
        public static Texture2D[] Deco_groundBottom;
        public static Texture2D[] Deco_groundMiddle;
        public static Texture2D[] Deco_stoneMiddle;
        public static Texture2D[] Deco_backgroundMiddle;
        public static Texture2D[] Deco_backgroundBottom;

        //ENTITIES
        public static Texture2D[] PlayerTextures;
        public static Texture2D[] PlayerWalkTextures;
        public static Texture2D[] Effect_PlayerParticlesBottom;
        public static Texture2D[] Effect_PlayerParticlesLeft;
        public static Texture2D[] Effect_PlayerParticlesRight;

        public static Texture2D[] PickupTexture_Coin;

        public static Texture2D[] EnemySlugTextures;
        public static Texture2D[] Spike_Up;
        public static Texture2D[] Spike_Down;
        public static Texture2D[] Spike_Left;
        public static Texture2D[] Spike_Right;

        //UI
        public static Texture2D[] UI_Hearts;
        public static Texture2D[] UI_MultiplierIndicators;
        public static Texture2D[] UI_Multipliers;

        public static Texture2D Logo;
        public static Texture2D[] SplashScreenGraphics;
        public static Texture2D Title;
        public static Texture2D[] MainMenuGraphics;
        public static Texture2D[] Icons;

        public static SpriteFont Font_Main;
        public static SpriteFont Font_Small;
        public static SpriteFont Font_Outlined;

        //OTHER
        public static Texture2D EmptyTexture;
        public static Texture2D BlackTexture;
        public static Texture2D DebugTexture;
        public static Texture2D[] ParallaxLayers;
        public static Texture2D Overlay;

        /// <summary>
        /// Loads all of the game's graphical content into static memory for
        /// lightweight referencing from anywhere in the code base.
        /// </summary>
        public void LoadContent(ContentManager Content) {
            this.Content = Content;

            Tiles_ground = PopulateTextureArray("tile_1", 16, @"graphics\tile\");
            Tiles_gold = PopulateTextureArray("tile_5", 16, @"graphics\tile\");
            Tiles_background = PopulateTextureArray("tile_2", 16, @"graphics\tile\");

            Deco_groundTop = PopulateTextureArray("deco_0", 12, @"graphics\deco\");
            Deco_groundBottom = PopulateTextureArray("deco_1", 3, @"graphics\deco\");
            Deco_groundMiddle = PopulateTextureArray("deco_4", 5, @"graphics\deco\");
            Deco_stoneMiddle = PopulateTextureArray("deco_5", 5, @"graphics\deco\");
            Deco_backgroundMiddle = PopulateTextureArray("deco_2", 3, @"graphics\deco\");
            Deco_backgroundBottom = PopulateTextureArray("deco_3", 3, @"graphics\deco\");

            ParallaxLayers = PopulateTextureArray("parallax", 2, @"graphics\misc\");

            PlayerTextures = PopulateTextureArray("player", 7, @"graphics\entities\");
            PlayerWalkTextures = PopulateTextureArray("playerWalk", 4, @"graphics\entities\");

            EnemySlugTextures = PopulateTextureArray("enemySlug", 5, @"graphics\entities\");
            Spike_Up = PopulateTextureArray("spike_0", 3, @"graphics\entities\");
            Spike_Down = PopulateTextureArray("spike_2", 3, @"graphics\entities\");
            Spike_Left = PopulateTextureArray("spike_1", 3, @"graphics\entities\");
            Spike_Right = PopulateTextureArray("spike_3", 3, @"graphics\entities\");

            Effect_PlayerParticlesBottom = PopulateTextureArray("playerParticles_0", 3, @"graphics\entities\");
            Effect_PlayerParticlesLeft = PopulateTextureArray("playerParticles_2", 3, @"graphics\entities\");
            Effect_PlayerParticlesRight = PopulateTextureArray("playerParticles_1", 3, @"graphics\entities\");

            PickupTexture_Coin = PopulateTextureArray("pickup_0", 4, @"graphics\entities\");

            EmptyTexture = Content.Load<Texture2D>(@"graphics\empty");
            BlackTexture = Content.Load<Texture2D>(@"graphics\ui\black");
            DebugTexture = Content.Load<Texture2D>(@"graphics\debug");
            Overlay = Content.Load<Texture2D>(@"graphics\overlay");

            UI_Hearts = PopulateTextureArray("heart", 3, @"graphics\ui\");
            UI_MultiplierIndicators = PopulateTextureArray("multiplierIndicator", 2, @"graphics\ui\");
            UI_Multipliers = PopulateTextureArray("multiplier", 8, @"graphics\ui\");

            Icons = PopulateTextureArray("icons", 2, @"graphics\ui\");

            Font_Main = Content.Load<SpriteFont>(@"graphics\ui\font");
            Font_Small = Content.Load<SpriteFont>(@"graphics\ui\font_small");
            Font_Outlined = Content.Load<SpriteFont>(@"graphics\ui\font_outlined");

            Logo = Content.Load<Texture2D>(@"graphics\ui\logo");
            Title = Content.Load<Texture2D>(@"graphics\ui\title");
            SplashScreenGraphics = PopulateTextureArray("splashscreen", 1, @"graphics\ui\");
        }

        /// <summary>
        /// Loads Content folder assets based on a given prefix string, number of textures
        /// with said prefix, and path.
        /// </summary>
        Texture2D[] PopulateTextureArray (string targetString, int numberOfTextures, string targetDirectoryString = "") {
            List<Texture2D> ret = new List<Texture2D>();

            for(int i = 0; i < numberOfTextures; i++)
                ret.Add(Content.Load<Texture2D>(targetDirectoryString + targetString + "_" + i));

            return ret.ToArray();
        }
    }
}
