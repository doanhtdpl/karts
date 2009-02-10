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
            PlayerManager.GetInstance().CreatePlayer(new Vector3(0.0f, 200.0f, 10000.0f), new Vector3(0.0f, 0.0f, 0.0f), 0.5f, "Barbur", "Ship", "Ship", true);
            CircuitManager.GetInstance().CreateCircuit(new Vector3(0.0f, 0.0f, 1000.0f), new Vector3(0.0f, 0.0f, 0.0f), "Ground");

            base.Enter();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.GetInstance().isButtonPressed(0, Buttons.B) || InputManager.GetInstance().isKeyPressed(Keys.Back))
            {
                GameStateManager.GetInstance().ChangeState(new MainMenu());
            }

            PlayerManager.GetInstance().Update(gameTime);
            CircuitManager.GetInstance().Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            PlayerManager.GetInstance().Draw(gameTime);
            CircuitManager.GetInstance().Draw(gameTime);
        }

        public override void Exit()
        {
            PlayerManager.GetInstance().RemovePlayers();
            base.Exit();
        }
    }
}
