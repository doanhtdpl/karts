using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class GameStateManager
    {
        // ------------------------------------------------
        // Class members
        // ------------------------------------------------
        private GameState m_CurrentGamestate;

        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public GameStateManager() 
        {
            m_CurrentGamestate = null;
        }

        ~GameStateManager()
        {

        }

        public void ChangeState(EGameStateType type)
        {
            if (type != EGameStateType.EGM_INVALID)
            {
                m_CurrentGamestate.Exit();
                m_CurrentGamestate = 
            }
        }

        public void Update (GameTime GameTime)
        {
            if (m_CurrentGamestate != null)
                m_CurrentGamestate.Update(GameTime);
        }

        public void Draw(GameTime GameTime)
        {
            if (m_CurrentGamestate != null)
                m_CurrentGamestate.Draw(GameTime);
        }

        // ------------------------------------------------
        // Predicates
        // ------------------------------------------------
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
