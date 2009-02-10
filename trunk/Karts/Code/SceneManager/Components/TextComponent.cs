﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Karts.Code.SceneManager.Components
{
    class TextComponent : Component
    {
        private String text;
        private SpriteFont font;
        private int depth = 0;

        public TextComponent(float x, float y, String text, String fontName)
            : base(x, y)
        {
            this.text = text;
            font = ResourcesManager.GetInstance().GetContentManager().Load<SpriteFont>(fontName);
        }

        public override void Draw(Vector2 parentPos, Vector2 parentScale)
        {
            if (Visible)
            {
                spriteBatch.DrawString(font, text, Position + parentPos, Color, Angle, Origin, Scale * parentScale, Effects, depth);
            }
        }
    }
}