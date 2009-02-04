using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class GameStateManager : DrawableGameComponent
    {
        private GameState m_CurrentGamestate;
        public static GameStateManager m_GameStateManager = null;

        public GameStateManager(Game game) : base(game)
        {
            m_CurrentGamestate = null;
        }

        ~GameStateManager()
        {
            m_CurrentGamestate = null;
        }

        public static GameStateManager Init(Game game)
        {
            if (m_GameStateManager == null)
                m_GameStateManager = new GameStateManager(game);
            return m_GameStateManager;
        }

        public static GameStateManager GetInstance()
        {
            return m_GameStateManager;
        }

        public void ChangeState(GameState state)
        {
            if (m_CurrentGamestate != null)
                m_CurrentGamestate.Exit();

            m_CurrentGamestate = state;

            if (m_CurrentGamestate != null)
            {
                m_CurrentGamestate.Enter();
            }
        }

        public override void Update (GameTime GameTime)
        {
            if (m_CurrentGamestate != null)
                m_CurrentGamestate.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {
            if (m_CurrentGamestate != null)
                m_CurrentGamestate.Draw(GameTime);
        }
    }
}
