using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code.Network
{
    class Server
    {
        public static Server m_Server = null;

        public static Server GetInstance()
        {
            return m_Server;
        }

        public static Server Init(Game game)
        {
            if (m_Server == null)
                m_Server = new Server(game);

            return m_Server;
        }

        private Server(Game game)
        {
        }
    }
}
