using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class GameStateManager
    {
        private List<GameState> m_GameStates = new List<GameState>();
        private GameState m_CurrentGamestate;

        public GameStateManager() 
        {
            m_CurrentGamestate = null;
        }

        ~GameStateManager()
        {
            if (m_GameStates.Count > 0)
                m_GameStates.Clear();
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
    }
}
