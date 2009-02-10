using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Karts.Code.SceneManager.Effects
{
    class ScriptEntry
    {
        private GuiEffect effect;
        private long startTime;
        private bool enabled;
        private bool started;

        public ScriptEntry(GuiEffect effect, long startTime)
        {
            this.effect = effect;
            this.startTime = startTime;
            enabled = false;
            started = false;
        }

        public void start()
        {
            enabled = true;
            started = false;
        }

        public void Update(long elapsed)
        {
            if(enabled){
                if(!started && startTime >= elapsed){
                    started = true;
                    effect.setEnabled(true);
                }
            }
        }

        public bool isDone()
        {
            return started && !effect.isEnabled() ;
        }
    }
}
