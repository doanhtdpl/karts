using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Karts.Code
{
    class CreateMultiplayerGame : GameState
    {
        public override EGameStateType GetStateType() { return EGameStateType.EGM_CREATE_MULTIPLAYER_GAME; }
        
        public override void Enter()
        {
            Karts.m_NetworkManager.CreateSession();
        }

        public override void Exit()
        {
            Karts.m_NetworkManager.DestroySession();
        }
    }
}
