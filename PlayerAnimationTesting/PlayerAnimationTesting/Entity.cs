using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PlayerAnimationTesting {
	abstract class Entity {

		Texture2D spriteSheet;



		public abstract void LoadSpriteSheet();

		public Rectangle[] GenerateSheetStrip(Texture2D sheet, int frameCount, int width, int height, int yOffset) {
			Rectangle[] frames = new Rectangle[frameCount];
			for (int i = 0; i < frameCount; i++) {
				frames[frameCount] = new Rectangle(width * i, yOffset, width, height);
			}
			return frames;
		}

	}
}
