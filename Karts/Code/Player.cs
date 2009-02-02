using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class Player
    {
        private Vehicle m_Vehicle;
        private Driver m_Driver;

        public void Update(GameTime GameTime)
        {
            m_Vehicle.Update(GameTime);
            m_Driver.Update(GameTime);
        }

        public void Draw(GameTime GameTime)
        {
            m_Vehicle.Draw(GameTime);
            m_Driver.Draw(GameTime);
        }
    }
}
