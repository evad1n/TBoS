using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Bond_of_Stone {
    /// <summary>
    /// Set it and forget it. This is the cloud layer behind the game stage.
    /// 
    /// By Dom Liotti
    /// </summary>
    class ParallaxLayer {
        private Texture2D texture;
        private Vector2 offset;

        private Viewport viewport;

        Vector2 baseSpeed;
        Vector2 speedDamper = new Vector2(0.025f, 0.01f);

        public Player target;

        private Rectangle Rect {
            get { return new Rectangle((int)(offset.X), (int)(offset.Y), (viewport.Width), (viewport.Height)); }
        }

        //Just give it a texture and a target, then render it to the game viewport (not the camera view matrix)
        public ParallaxLayer(Texture2D texture, Player target, Vector2 baseSpeed, Viewport viewport) {
            this.texture = texture;
            offset = Vector2.Zero;
            this.target = target;
            this.baseSpeed = baseSpeed;

            this.viewport = viewport;
        }

        public void Update(GameTime gameTime) {
            float e = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 newDisp = new Vector2();

            if (target != null)
                newDisp = new Vector2(target.velocity.X, 0) * e;

            newDisp = new Vector2(newDisp.X + baseSpeed.X, newDisp.Y + baseSpeed.Y) * speedDamper;

            offset += newDisp;
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(texture, new Vector2(viewport.X, viewport.Y), Rect, Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 1);
        }
    }
}
