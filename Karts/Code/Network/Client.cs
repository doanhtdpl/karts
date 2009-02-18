using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code.Network
{
    class Client
    {
        public static Client m_Client = null;

        public static Client GetInstance()
        {
            return m_Client;
        }

        public static Client Init(Game game)
        {
            if (m_Client == null)
                m_Client = new Client(game);

            return m_Client;
        }

        private Client(Game game)
        {
        }
    }
}
