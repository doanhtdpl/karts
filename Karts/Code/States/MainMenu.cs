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
using Karts.Code.SceneManager.Effects;
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
            //Guide.ShowSignIn(1, false);

            for (int i = 0; i < options.Length; ++i)
            {
                options[i] = new TextComponent(100, 100 * (i + 1), OPTIONS[i], "KartsFont");
                menu.AddComponent(options[i]);
                FaderEffect fader = new FaderEffect(options[i], 2000, false, 0, 255);
                Gui.GetInstance().AddEffect(fader);
                fader.setEnabled(true);
            }

            Gui.GetInstance().AddComponent(menu);

            UpdateSelected();

            base.Enter();
        }

        public override void Update(GameTime GameTime)
        {
            if(ControllerManager.GetInstance().isPressed("menu_down"))
            {
                selected = (selected + OPTIONS.Length + 1) % OPTIONS.Length;
                UpdateSelected();
            }
            else if (ControllerManager.GetInstance().isPressed("menu_up"))
            {
                selected = (selected + OPTIONS.Length - 1) % OPTIONS.Length;
                UpdateSelected();
            }
            else 
            {
                for (int i = 0; i < PlayerManager.MAX_LOCAL_PLAYERS; ++i)
                {
                    if (ControllerManager.GetInstance().isPressed(i, "menu_ok"))
                    {
                        PlayerManager.GetInstance().ActivePlayerIndex = i;

                        if (selected == 0)
                        {
                            GameStateManager.GetInstance().ChangeState(new CreateLocalGame());
                        }
                        else if (selected == 1)
                        {
                            GameStateManager.GetInstance().ChangeState(new CreateMultiplayerGame());
                        }
                        else if (selected == 2)
                        {
                            GameStateManager.GetInstance().ChangeState(new FindMultiplayerGame());
                        }
                        break;
                    }
                }
            }

            base.Update(GameTime);
        }

        private void UpdateSelected()
        {
            for (int i = 0; i < options.Length; ++i)
            {
                options[i].Color = i == selected ? Color.Red : Color.White;
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
