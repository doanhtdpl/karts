using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Karts.Code
{
    class GameplayState : GameState
    {

        public override void Enter()
        {
            PlayerManager.GetInstance().CreatePlayer(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), "Barbur", "Ship", "Ship");

            base.Enter();
        }

        public override void Update(GameTime GameTime)
        {
            if (InputManager.GetInstance().isButtonPressed(0, Buttons.B) || InputManager.GetInstance().isKeyPressed(Keys.Back))
            {
                GameStateManager.GetInstance().ChangeState(new MainMenu());
            }

            base.Update(GameTime);
        }

        public override void Exit()
        {
            PlayerManager.GetInstance().RemovePlayers();
            base.Exit();
        }
    }
}
