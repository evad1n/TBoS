using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBondOfStone {
	public class PlayerStats {

		private int health;
		private int scoreBits;
		private int multiplier;

		public int PlayerScore { get; set; }
		public int Health {
			get { return health; }
			set {
				if (value <= 6 && value >= 0)
					health = value;
			}
		}
		public int ScoreBits {
			get { return scoreBits; }
			set {
				if (value > 0 && value <= 4)
					scoreBits = value;
			}
		}
		public int Multiplier {
			get { return multiplier; }
			set {
				if (value <= 4 && value >= 1)
					multiplier = value;
			}
		}

		public PlayerStats() {
			health = 6;
			scoreBits = 0;
			multiplier = 1;
		}

        //Reset all values to game start
        public void Reset()
        {
            PlayerScore = 0;
            health = 6;
            scoreBits = 0;
            multiplier = 1;
        }

	}
}
