using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Karts.Code.SceneManager.Components
{
    class Component
    {
        protected SpriteBatch spriteBatch;

        public float Angle { set; get; }
        public bool Visible { set; get; }
        public Vector2 Position { set; get; }
        public Vector2 Origin { set; get; }
        public Vector2 Scale { set; get; }
        public Color Color { set; get; }
        public Texture2D Texture { set; get; }
        public SpriteEffects Effects { set; get; }
        public int Depth { set; get; }
        public Rectangle Source { set; get; }

        public Component() { this.spriteBatch = Gui.GetInstance().GetSpriteBatch(); }

        public Component(float x, float y)
            : this()
        {
            Position = new Vector2(x, y);
            Visible = true;
            Origin = new Vector2();
            Scale = new Vector2(1, 1);
            Color = Color.Black;
            Effects = SpriteEffects.None;
            Depth = 0;
        }

        public Component(float x, float y, String textureName)
            : this(x,y)
        {
            Texture = ResourcesManager.GetInstance().GetContentManager().Load<Texture2D>(textureName);
        }

        public virtual void Draw(float x, float y)
        {
            if (Visible)
            {
                spriteBatch.Draw(Texture, Position, Source, Color, Angle, Origin, Scale, Effects, Depth);
            }
        }

    }
}
