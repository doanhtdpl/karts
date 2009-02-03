using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    // ------------------------------------------------
    // Types
    // ------------------------------------------------
    enum EGameStateType
    {
        EGM_INVALID     = 0,
        EGM_MAIN_MENU   = 1,
        EGM_PAUSE_MENU  = 2,
        EGM_GAME        = 3
    }

    class GameState
    {
        // ------------------------------------------------
        // Class members
        // ------------------------------------------------
        private bool            m_bActive;
        private EGameStateType  m_eType;


        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public GameState() 
        {
            m_bActive = false;
            m_eType = EGameStateType.EGM_INVALID;
        }

        ~GameState() { }

        public bool IsActive()  { return m_bActive; }
        public EGameStateType GetType()   { return m_eType; }

        public void Update(GameTime GameTime)
        {
        }

        public void Draw(GameTime GameTime)
        {
        }
    }
}
