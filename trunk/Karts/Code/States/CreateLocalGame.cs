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
    class CreateLocalGame : GameState
    {
        private Screen menu;

        public override void Enter()
        {
            menu = new Screen();
            Gui.GetInstance().AddComponent(menu);

            menu.AddComponent(new TextComponent(200, 100, "CREATE LOCAL GAME", "kartsFont"));
            menu.AddComponent(new TextComponent(150, 300, "PRESS BUTTON TO CONFIRM", "kartsFont"));
        }

        public override void Update(GameTime GameTime)
        {
            if (ControllerManager.GetInstance().isPressed("menu_ok")){
                //GameStateManager.GetInstance().ChangeState(new GameplayState());

                //Create main player
                PlayerManager.GetInstance().RemovePlayers();
                PlayerManager.GetInstance().CreatePlayer("Player" + PlayerManager.GetInstance().ActivePlayerIndex, true, false, PlayerManager.GetInstance().ActivePlayerIndex);

                GameStateManager.GetInstance().ChangeState(new Lobby());
            }
            else if (ControllerManager.GetInstance().isPressed("menu_back"))
            {
                GameStateManager.GetInstance().ChangeState(new MainMenu());
            }
            base.Update(GameTime);
        }

        public override void Exit()
        {
            Gui.GetInstance().RemoveComponent(menu);
        }
    }
}
