using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    enum EGameStateType
    {
        EGM_INVALID     = 0,
        EGM_MAIN_MENU = 1,
        EGM_CREATE_MULTIPLAYER_GAME = 3,
        EGM_FIND_MULTIPLAYER_GAME = 4,
        EGM_GAME = 2
    }

    class GameState
    {
        private bool            m_bActive;

        public GameState() 
        {
            m_bActive = false;
        }

        ~GameState() { }

        public bool IsActive()  { return m_bActive; }
        public virtual EGameStateType GetStateType() { return EGameStateType.EGM_INVALID; }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void Update(GameTime GameTime)
        {
        }

        public virtual void Draw(GameTime GameTime)
        {
        }
    }
}
