using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBondOfStone
{
    class ParallaxLayer
    {
        private Texture2D texture;
        private Vector2 offset;
        public Vector2 disp;
        
        private Viewport viewport;

        Vector2 baseSpeed;

        private Rectangle Rect {
            get { return new Rectangle((int)(offset.X), (int)(offset.Y), (viewport.Width), (viewport.Height)); }
        }

        public ParallaxLayer(Texture2D texture, Vector2 disp, Vector2 baseSpeed, Viewport viewport) {
            this.texture = texture;
            offset = Vector2.Zero;
            this.disp = disp;
            this.baseSpeed = baseSpeed;

            this.viewport = viewport;
        }

        public void Update(GameTime gameTime) {
            float e = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 newDisp = disp * e;
            newDisp = new Vector2(newDisp.X + baseSpeed.X, newDisp.Y + baseSpeed.Y);

            offset += newDisp;
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(texture, new Vector2(viewport.X, viewport.Y), Rect, Color.White, 0, Vector2.Zero, Game1.PixelScaleFactor/8, SpriteEffects.None, 1);
        }
    }
}
