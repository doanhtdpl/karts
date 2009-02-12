using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Karts.Code.SceneManager.Components
{
    class TextComponent : Component
    {
        public String Text{ get; set;}
        private SpriteFont font;

        public TextComponent(float x, float y, String text, String fontName)
            : base(x, y)
        {
            Text = text;
            font = ResourcesManager.GetInstance().GetContentManager().Load<SpriteFont>(fontName);
        }

        public override void Draw(Vector2 parentPos, Vector2 parentScale)
        {
            if (Visible)
            {
                spriteBatch.DrawString(font, Text, Position + parentPos, Color, Angle, Origin, Scale * parentScale, Effects, Depth);
            }
        }
    }
}
