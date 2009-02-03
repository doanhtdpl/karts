using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class PlayerManager
    {
        //------------------------------------------- 
        // Class members
        //------------------------------------------- 
        private List<Player> m_PlayerList = new List<Player>();

        //------------------------------------------- 
        // Class methods
        //------------------------------------------- 
        public PlayerManager() { }
        ~PlayerManager()
        {
            m_PlayerList.Clear();
        }

        public Player GetPlayer(UInt32 id) { return m_PlayerList.Find(new PredComparePlayerID(id).EqualsID); }

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

        //------------------------------------------- 
        // Predicates
        //------------------------------------------- 
        public struct PredComparePlayerID
        {
            private UInt32 id;
            public PredComparePlayerID(UInt32 _id) { id = _id; }

            public bool EqualsID(Player p)
            {
                return p.GetID() == id;
            }
        }
    }
}
