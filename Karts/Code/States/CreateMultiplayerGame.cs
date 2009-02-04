using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Enter) && session.AllGamers.Count > 1){
                session.StartGame();
                GameStateManager.GetInstance().ChangeState(new GameplayState());
            }else if (state.IsKeyDown(Keys.Escape)){
                session.Dispose();
                GameStateManager.GetInstance().ChangeState(new MainMenu());
            }
            base.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {
            for (int i = 0; i < session.AllGamers.Count; ++i)
            {
                DrawDebugManager.GetInstance().DrawText(session.AllGamers[i].Gamertag, 100, 100 + i * 100, Color.Black);
            }

            base.Draw(GameTime);
        }

        public override void Exit()
        {
        }
    }
}
