using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

namespace Karts.Code
{
    class MainMenu : GameState
    {
        private String[] OPTIONS = { "CREATE LOCAL GAME", "CREATE MULTIPLAYER GAME", "FIND MULTIPLAYER GAME", "EXIT GAME" };

        private int selected = 0;

        public override void Enter()
        {
            Guide.ShowSignIn(1, false);

            base.Enter();
        }

        public override void Update(GameTime GameTime)
        {
            if (InputManager.GetInstance().isInputPressed(0, Buttons.DPadDown) || InputManager.GetInstance().getAxisY(0, true) < 0){
                selected = (selected + OPTIONS.Length + 1) % OPTIONS.Length;
            }
            else if (InputManager.GetInstance().isInputPressed(0, Buttons.DPadUp) || InputManager.GetInstance().getAxisY(0, true) > 0){
                selected = (selected + OPTIONS.Length - 1) % OPTIONS.Length;
            }
            else if (InputManager.GetInstance().isInputPressed(0, Buttons.A))
            {
                if(selected == 3){
                    Karts.karts.Exit();
                }
                else if (selected == 0)
                {
                    GameStateManager.GetInstance().ChangeState(new CreateMultiplayerGame());
                }
                else if (selected == 1)
                {
                    GameStateManager.GetInstance().ChangeState(new FindMultiplayerGame());
                }
                else if (selected == 2)
                {
                    GameStateManager.GetInstance().ChangeState(new GameplayState());
                }
            }

            base.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {
            for (int i = 0; i < OPTIONS.Length; ++i)
            {
                DrawDebugManager.GetInstance().DrawText(OPTIONS[i], 100, 100 + i * 100, i == selected ? Color.Red : Color.Black);
            }
            base.Draw(GameTime);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
