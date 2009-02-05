using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Karts.Code.SceneManager;
using Karts.Code.SceneManager.Components;

namespace Karts.Code
{
    class WaitForOtherPlayers : GameState
    {
        private Screen menu;

        public override void Enter()
        {
            menu = new Screen();
            Gui.GetInstance().AddComponent(menu);
            base.Enter();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime GameTime)
        {
            if (InputManager.GetInstance().isKeyPressed(Keys.Back))
            {
                NetworkManager.GetInstance().LeaveSession();
                GameStateManager.GetInstance().ChangeState(new FindMultiplayerGame());
            }else if (NetworkManager.GetInstance().GetSession().SessionState == NetworkSessionState.Playing)
            {
                GameStateManager.GetInstance().ChangeState(new GameplayState());
            }
            base.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {
            menu.RemoveAll();
            NetworkSession session = NetworkManager.GetInstance().GetSession();
            for (int i = 0; i < session.AllGamers.Count; ++i)
            {
                menu.AddComponent(new TextComponent(100, 100 * (i + 1), session.AllGamers[i].Gamertag, "KartsFont"));
            }
            base.Draw(GameTime);
        }

        public override void Exit()
        {
            Gui.GetInstance().RemoveComponent(menu);
            base.Exit();
        }
    }
}
