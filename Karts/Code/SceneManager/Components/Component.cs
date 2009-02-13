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
        public float Depth { set; get; }
        public Rectangle Source { set; get; }

        public Component(float x, float y)
        {
            spriteBatch = Gui.GetInstance().GetSpriteBatch();

            Position = new Vector2(x, y);
            Visible = true;
            Origin = new Vector2();
            Scale = Vector2.One;
            Color = Color.White;
            Effects = SpriteEffects.None;
            Depth = 0;
        }

        public Component(float x, float y, String textureName)
            : this(x,y)
        {
            Texture = ResourcesManager.GetInstance().GetContentManager().Load<Texture2D>(textureName);
        }

        public virtual void Draw(Vector2 parentPos, Vector2 parentScale)
        {
            if (Visible && Texture != null)
            {
                spriteBatch.Draw(Texture, Position + parentPos, Source, Color, Angle, Origin, Scale * parentScale, Effects, Depth);
            }
        }

    }
}
