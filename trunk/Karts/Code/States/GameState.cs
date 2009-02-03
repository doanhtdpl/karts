using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class GameState
    {
        private bool            m_bActive;

        public GameState() 
        {
            m_bActive = false;
        }

        ~GameState() { }

        public bool IsActive()  { return m_bActive; }

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
