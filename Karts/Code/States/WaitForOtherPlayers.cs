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
            Karts.spriteBatch.Begin();
            for (int i = 0; i < NetworkManager.GetInstance().GetSession().AllGamers.Count; ++i)
            {
                Karts.spriteBatch.DrawString(Karts.spriteFont, NetworkManager.GetInstance().GetSession().AllGamers[i].Gamertag, new Vector2(100, 100 + i * 100), Color.Black);
            }
            Karts.spriteBatch.End();
            base.Draw(GameTime);
        }
    }
}
