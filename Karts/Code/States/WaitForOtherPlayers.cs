using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class WaitForOtherPlayers : GameState
    {
        public override void Update(Microsoft.Xna.Framework.GameTime GameTime)
        {
            if (NetworkManager.GetInstance().GetSession().SessionState == NetworkSessionState.Playing)
            {
                GameStateManager.GetInstance().ChangeState(new GameplayState());
            }
            base.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {
            for (int i = 0; i < NetworkManager.GetInstance().GetSession().AllGamers.Count; ++i)
            {
                DrawDebugManager.GetInstance().DrawText(NetworkManager.GetInstance().GetSession().AllGamers[i].Gamertag, 100, 100 + i * 100, Color.Black);
            }
            base.Draw(GameTime);
        }
    }
}
