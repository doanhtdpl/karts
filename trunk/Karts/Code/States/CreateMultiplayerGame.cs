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
using Microsoft.Xna.Framework.GamerServices;

namespace Karts.Code
{
    class CreateMultiplayerGame : GameState
    {
        private NetworkSession session = null;
        private Screen menu;

        public override void Enter()
        {
            if (Gamer.SignedInGamers.Count == 0)
                Guide.ShowSignIn(1, false);

            menu = new Screen();
            Gui.GetInstance().AddComponent(menu);

            menu.AddComponent(new TextComponent(200, 100, "CREATE MULTIPLAYER GAME", "kartsFont"));
            menu.AddComponent(new TextComponent(150, 300, "PRESS BUTTON TO CONFIRM", "kartsFont"));
        }

        public override void Update(GameTime GameTime)
        {
            if (ControllerManager.GetInstance().isPressed("menu_ok"))
            {
                //Create main player
                PlayerManager.GetInstance().RemovePlayers();
                PlayerManager.GetInstance().CreatePlayer("Player" + PlayerManager.GetInstance().ActivePlayerIndex, true, false, PlayerManager.GetInstance().ActivePlayerIndex);

                session = NetworkManager.GetInstance().CreateSession();

                GameStateManager.GetInstance().ChangeState(new Lobby());
            }
            else if (ControllerManager.GetInstance().isPressed("menu_back"))
            {
                GameStateManager.GetInstance().ChangeState(new MainMenu());
            }
            base.Update(GameTime);
            /*
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Enter) && session.AllGamers.Count > 1){
                session.StartGame();
                GameStateManager.GetInstance().ChangeState(new GameplayState());
            }else if (state.IsKeyDown(Keys.Back)){
                session.Dispose();
                GameStateManager.GetInstance().ChangeState(new MainMenu());
            }
            base.Update(GameTime);
             */
        }

        public override void Exit()
        {
            Gui.GetInstance().RemoveComponent(menu);
        }
    }
}
