using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game3
{
    class Bullets
    {
        public Texture2D texture;
        public Vector2 position, velocity, origin;

        public bool isVisible;

        public Bullets(Texture2D newTexture)
        {
            texture = newTexture;
            isVisible = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0);
        }
        public Rectangle getArea(Texture2D temp)
        {
            Rectangle area = new Rectangle((int)position.X - temp.Width / 2, (int)position.Y - temp.Height / 2,
                                    temp.Width, temp.Height);
            return area;
        }
    }
}
