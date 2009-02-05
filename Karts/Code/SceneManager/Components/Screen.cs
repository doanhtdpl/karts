using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Karts.Code.SceneManager.Components
{
    class Screen : Container
    {
        public Screen() : base(0, 0) { }
        public Screen(float x, float y) : base(x, y) { }
    }
}
