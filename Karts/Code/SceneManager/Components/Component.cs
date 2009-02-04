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

        protected Vector2 position = new Vector2();
        protected float angle = 0;
        protected Vector2 origin = new Vector2();
        protected Vector2 scale = new Vector2(1, 1);
        protected bool visible = true;
        protected Color color = Color.Black;

        public Component() { this.spriteBatch = Gui.GetInstance().GetSpriteBatch(); }

        public Component(float x, float y) : this() {
            position.X = x;
            position.Y = y;
        }

        public virtual void Draw()
        {
            Draw(0, 0);
        }

        public virtual void Draw(float x, float y)
        {
            if (visible)
            {
            }
        }

        public bool IsVisible() { return visible; }

        public void setColor(Color color) { this.color = color; }
    }
}
