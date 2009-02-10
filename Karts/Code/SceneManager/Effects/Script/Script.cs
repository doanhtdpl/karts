using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Karts.Code.SceneManager.Effects
{
    class Script
    {
        private List<ScriptEntry> entries;
        private String nextScript;
        private long startTime;

        public Script()
        {
            entries = new List<ScriptEntry>();
            nextScript = null;
        }

        public void Update(long elapsed)
        {
            if (startTime == -1)
            {
                startTime = elapsed;
            }

            foreach (ScriptEntry entry in entries)
            {
                entry.Update(elapsed - startTime);
            }
        }

        public String getNextScript()
        {
            return nextScript;
        }

        public bool isFinished()
        {
            foreach (ScriptEntry entry in entries)
            {
                if (!entry.isDone())
                {
                    return false;
                }
            }
            return true;
        }

        public void start()
        {
            startTime = -1;
        }
    }
}
