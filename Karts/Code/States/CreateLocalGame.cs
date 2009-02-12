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


        }

        public override void Update(GameTime GameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Enter)){
                GameStateManager.GetInstance().ChangeState(new GameplayState());
            }else if (state.IsKeyDown(Keys.Back)){
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
    }
}
