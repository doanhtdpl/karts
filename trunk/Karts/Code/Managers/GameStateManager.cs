using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class GameStateManager
    {
        private GameState m_CurrentGamestate;
        public static GameStateManager m_GameStateManager = null;

        public GameStateManager() 
        {
            m_CurrentGamestate = null;
        }

        ~GameStateManager()
        {
            m_CurrentGamestate = null;
        }

        public static GameStateManager GetInstance()
        {
            if (m_GameStateManager == null)
                m_GameStateManager = new GameStateManager();

            return m_GameStateManager;
        }

        public void ChangeState(EGameStateType type)
        {
            if (m_CurrentGamestate != null)
                m_CurrentGamestate.Exit();

            m_CurrentGamestate = CreateState(type);

            if (m_CurrentGamestate != null)
            {
                m_CurrentGamestate.Enter();
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

        private GameState CreateState(EGameStateType type)
        {
            if (type == EGameStateType.EGM_CREATE_MULTIPLAYER_GAME)
                return new CreateMultiplayerGame();
            else if (type == EGameStateType.EGM_MAIN_MENU)
                return new MainMenu();
            else if (type == EGameStateType.EGM_FIND_MULTIPLAYER_GAME)
                return new FindMultiplayerGame();
            else
                return null;
        }
    }
}
