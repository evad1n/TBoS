using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.IO;
using MonoGame.Extended;

namespace TheBondOfStone {
	static class SpriteSheetManager {

		public static Rectangle[] Split(int frameWidth, int frameHeight, int numFrames, int startX = 0, int startY = 0) {
			Rectangle[] frames = new Rectangle[numFrames];
			for (int i = 0; i < numFrames; i++) {
				frames[i] = new Rectangle(frameWidth * i + startX, startY, frameWidth, frameHeight);
			}
			return frames;
		}
	}
}
