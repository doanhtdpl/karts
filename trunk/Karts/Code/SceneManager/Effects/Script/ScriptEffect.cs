using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Karts.Code.SceneManager.Components;

namespace Karts.Code.SceneManager.Effects
{
    class ScriptEffect : GuiEffect
    {
        protected Dictionary<String, Script> scripts;
        protected Script actualScript;
        protected String initScriptName;

        public ScriptEffect(Component comp, long duration, bool loop, String initScriptName):base(comp, duration, loop)
        {
            scripts = new Dictionary<String, Script>();
            this.initScriptName = initScriptName;
        }

        public override void effectFinished()
        {
        }

        public void setInitScript(String script)
        {
            initScriptName = script;
        }

        public override void enablePropertyChanged(bool value)
        {
            if (value)
            {
                setScript(initScriptName);
            }
        }

        public override void UpdateEffect(long elapsed, float perc)
        {
            actualScript.Update(elapsed);
            if (actualScript.isFinished())
            {
                if (actualScript.getNextScript() != null)
                {
                    setScript(actualScript.getNextScript());
                }
                else
                {
                    setEnabled(false);
                }
            }
        }

        public void setScript(String script)
        {
            actualScript = scripts[script];
            actualScript.start();
        }

        public bool isFinished()
        {
            return !isEnabled() && (actualScript == null || actualScript.isFinished());
        }
    }
}
