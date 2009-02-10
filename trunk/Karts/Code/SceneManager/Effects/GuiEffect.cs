using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Karts.Code.SceneManager.Components;

namespace Karts.Code.SceneManager.Effects
{
    class GuiEffect 
    {
        protected Component component;
        protected bool enabled;
        protected bool loop;
        protected long duration;
        protected long elapsedTime;

        public GuiEffect(Component comp, long duration, bool loop)
        {
            this.component = comp;
            this.duration = duration;
            this.loop = loop;
            this.enabled = false;
            this.elapsedTime = 0;
        }

        public void setEnabled(bool value)
        {
            this.enabled = value;
            enablePropertyChanged(value);
            elapsedTime = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (enabled)
            {
                elapsedTime = elapsedTime + gameTime.ElapsedGameTime.Milliseconds;

                if (elapsedTime >= duration && !loop)
                {
                    effectFinished();
                    setEnabled(false);
                }
                else
                {
                    elapsedTime = elapsedTime % duration;
                    float perc = (float)elapsedTime / (float) duration;
                    UpdateEffect(elapsedTime, perc);
                }
            }
        }

        public bool isEnabled()
        {
            return enabled;
        }

        public virtual void effectFinished()
        {
        }

        public virtual void enablePropertyChanged(bool value)
        {
        }

        public virtual void UpdateEffect(long elapsed, float perc)
        {
        }
    }
}
