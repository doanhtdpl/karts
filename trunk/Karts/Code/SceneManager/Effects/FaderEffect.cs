using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Karts.Code.SceneManager.Components;

namespace Karts.Code.SceneManager.Effects
{
    class FaderEffect : GuiEffect
    {
        public byte initAlpha;
        public byte endAlpha;
        private byte dif;

        public FaderEffect(Component comp, long duration, bool loop, byte initAlpha, byte endAlpha):base(comp, duration, loop){
            this.initAlpha = initAlpha;
            this.endAlpha = endAlpha;
            dif = (byte) (endAlpha - initAlpha);
        }

        public override void enablePropertyChanged(bool value)
        {
            if (value)
            {
                Color color = component.Color;
                color.A = initAlpha;
                component.Color = color;
            }
        }

        public override void UpdateEffect(long elapsed, float perc)
        {
            Color color = component.Color;
            color.A = (byte) (initAlpha + (dif * perc));
            component.Color = color;
        }

        public override void effectFinished()
        {
            Color color = component.Color;
            color.A = endAlpha;
            component.Color = color;
        }
    }
}
