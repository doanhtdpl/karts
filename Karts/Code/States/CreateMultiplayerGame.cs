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
            NetworkManager.GetInstance().CreateSession();
        }

        public override void Exit()
        {
            NetworkManager.GetInstance().DestroySession();
        }
    }
}
