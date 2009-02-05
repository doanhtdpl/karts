using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Karts.Code.SceneManager.Components
{
    class Container : Component
    {
        private List<Component> components = new List<Component>();

        public Container(float x, float y)
            : base(x, y)
        {
        }

        public override void Draw(Vector2 parentPos, Vector2 parentScale)
        {
            base.Draw(Position + parentPos, Scale * parentScale);

            foreach (Component component in components)
                component.Draw(Position + parentPos, Scale * parentScale);
        }

        public void AddComponent(Component comp)
        {
            components.Add(comp);
        }

        public void RemoveComponent(Component comp)
        {
            components.Remove(comp);
        }

        public void RemoveAll()
        {
            components.Clear();
        }

    }

}
