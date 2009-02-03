using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class PlayerManager
    {
        private List<Player> m_PlayerList = new List<Player>();
        public static PlayerManager m_PlayerManager = null;

        public PlayerManager() { }
        ~PlayerManager()
        {
            m_PlayerList.Clear();
        }

        public static PlayerManager GetInstance()
        {
            if (m_PlayerManager == null)
                m_PlayerManager = new PlayerManager();

            return m_PlayerManager;
        }

        public void Update(GameTime GameTime)
        {
            foreach (Player p in m_PlayerList)
                p.Update(GameTime);
	    }

        public void Draw(GameTime GameTime)
        {
            foreach (Player p in m_PlayerList)
                p.Draw(GameTime);
        }
    }
}
