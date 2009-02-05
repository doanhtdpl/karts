﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Karts.Code.SceneManager.Components
{
    class Container : Component
    {
        private List<Component> components = new List<Component>();

        public override void Draw(float x, float y)
        {
            base.Draw(x + Position.X, y + Position.Y);

            foreach (Component component in components)
                component.Draw(x + Position.X, y + Position.Y);
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
