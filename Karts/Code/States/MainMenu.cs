using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Karts.Code.SceneManager.Components;
using Karts.Code.SceneManager;

namespace Karts.Code
{
    class MainMenu : GameState
    {
        private String[] OPTIONS = { "CREATE LOCAL GAME", "CREATE MULTIPLAYER GAME", "FIND MULTIPLAYER GAME" };
        private Component[] options = new Component[3];
        private Screen menu = new Screen();

        private int selected = 0;

        public override void Enter()
        {
            Guide.ShowSignIn(1, false);

            for (int i = 0; i < 4; ++i)
            {
                options[i] = new TextComponent(100, 100 * (i + 1), OPTIONS[i], "KartsFont");
                menu.AddComponent(options[i]);
            }

            Gui.GetInstance().AddComponent(menu);

            UpdateSelected();

            base.Enter();
        }

        public override void Update(GameTime GameTime)
        {
            if (InputManager.GetInstance().isButtonPressed(0, Buttons.DPadDown) || InputManager.GetInstance().isKeyPressed(Keys.Down))
            {
                selected = (selected + OPTIONS.Length + 1) % OPTIONS.Length;
                UpdateSelected();
            }
            else if (InputManager.GetInstance().isButtonPressed(0, Buttons.DPadUp) || InputManager.GetInstance().isKeyPressed(Keys.Up))
            {
                selected = (selected + OPTIONS.Length - 1) % OPTIONS.Length;
                UpdateSelected();
            }
            else if (InputManager.GetInstance().isButtonPressed(0, Buttons.A))
            {
                if (selected == 1)
                {
                    GameStateManager.GetInstance().ChangeState(new CreateMultiplayerGame());
                }
                else if (selected == 2)
                {
                    GameStateManager.GetInstance().ChangeState(new FindMultiplayerGame());
                }
                else if (selected == 0)
                {
                    GameStateManager.GetInstance().ChangeState(new GameplayState());
                }
            }

            base.Update(GameTime);
        }

        private void UpdateSelected()
        {
            for (int i = 0; i < options.Length; ++i)
            {
                options[i].Color = i == selected ? Color.Red : Color.Black;
            }
        }

        public override void Draw(GameTime GameTime)
        {
            base.Draw(GameTime);
        }

        public override void Exit()
        {
            Gui.GetInstance().RemoveComponent(menu);
            base.Exit();
        }
    }
}
