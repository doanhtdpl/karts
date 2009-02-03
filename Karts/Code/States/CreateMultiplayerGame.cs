using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class CreateMultiplayerGame : GameState
    {
        private NetworkSession session = null;

        public override void Enter()
        {
            session = NetworkManager.GetInstance().CreateSession();
        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {

            base.Draw(GameTime);
        }

        public override void Exit()
        {
        }
    }
}
