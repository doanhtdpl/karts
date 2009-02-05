using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Karts.Code.SceneManager;
using Karts.Code.SceneManager.Components;

namespace Karts.Code
{
    class CreateMultiplayerGame : GameState
    {
        private NetworkSession session = null;
        private Screen menu;

        public override void Enter()
        {
            menu = new Screen();
            Gui.GetInstance().AddComponent(menu);

            session = NetworkManager.GetInstance().CreateSession();
            session.GamerJoined += new EventHandler<GamerJoinedEventArgs>(session_GamerJoined);
            session.GamerLeft += new EventHandler<GamerLeftEventArgs>(session_GamerLeft);
        }

        public override void Update(GameTime GameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Enter) && session.AllGamers.Count > 1){
                session.StartGame();
                GameStateManager.GetInstance().ChangeState(new GameplayState());
            }else if (state.IsKeyDown(Keys.Back)){
                session.Dispose();
                GameStateManager.GetInstance().ChangeState(new MainMenu());
            }
            base.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {
            base.Draw(GameTime);
        }

        public override void Exit()
        {
            Gui.GetInstance().RemoveComponent(menu);
        }

        void session_GamerJoined(object sender, GamerJoinedEventArgs p)
        {
            menu.AddComponent(new TextComponent(100, 100 * (session.AllGamers.Count + 1), p.Gamer.Gamertag, "KartsFont"));
        }

        void session_GamerLeft(object sender, GamerLeftEventArgs p)
        {
            menu.RemoveAll();
            for (int i = 0; i < session.AllGamers.Count; ++i)
            {
                menu.AddComponent(new TextComponent(100, 100 * (i + 1), session.AllGamers[i].Gamertag, "KartsFont"));
            }
        }
    }
}
