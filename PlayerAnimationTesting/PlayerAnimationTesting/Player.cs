using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PlayerAnimationTesting {

	enum PlayerStates { Stand, Walk, Roll, Jump, Midair };
	class Player {

		// animation is (width per frame (in pixels), height per frame(in pixels), number of frames, 
		Point[] animations;

		public Player(Texture2D spritesheet) {
			animations[(int)PlayerStates.Stand] = new Point(2,0);
			animations[(int)PlayerStates.Walk] = new Point(3, 1);
			animations[(int)PlayerStates.Stand] = new Point(3, 2);
			animations[(int)PlayerStates.Stand] = new Point(1, 3);
			animations[(int)PlayerStates.Stand] = new Point(4, 4);
		}

		public void Draw(SpriteBatch sb) {
			
		}
	}
}
