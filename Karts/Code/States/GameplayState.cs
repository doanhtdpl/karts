using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Karts.Code
{
    class GameplayState : GameState
    {
        Viewport v1;
        Viewport v2;
        Viewport v3;
        Viewport v4;

        public override void Enter()
        {
            PlayerManager.GetInstance().CreatePlayer(new Vector3(0.0f, 200.0f, 10000.0f), new Vector3(0.0f, 0.0f, 0.0f), 0.5f, "Barbur", "Ship", "Ship", true);
            CircuitManager.GetInstance().CreateCircuit(new Vector3(0.0f, 0.0f, 1000.0f), new Vector3(0.0f, 0.0f, 0.0f), "Ground");

            v1 = new Viewport();
            v2 = new Viewport();
            v3 = new Viewport();
            v4 = new Viewport();

            v1.X = 0;
            v1.Y = 0;
            v1.Width = 400;
            v1.Height = 300;

            v2.X = 400;
            v2.Y = 0;
            v2.Width = 400;
            v2.Height = 300;

            v3.X = 0;
            v3.Y = 300;
            v3.Width = 400;
            v3.Height = 300;

            v4.X = 400;
            v4.Y = 300;
            v4.Width = 400;
            v4.Height = 300;

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
            ResourcesManager.GetInstance().GetGraphicsDeviceManager().GraphicsDevice.Viewport = v1;
            PlayerManager.GetInstance().Draw(gameTime);
            CircuitManager.GetInstance().Draw(gameTime);

            ResourcesManager.GetInstance().GetGraphicsDeviceManager().GraphicsDevice.Viewport = v2;
            PlayerManager.GetInstance().Draw(gameTime);
            CircuitManager.GetInstance().Draw(gameTime);

            ResourcesManager.GetInstance().GetGraphicsDeviceManager().GraphicsDevice.Viewport = v3;
            PlayerManager.GetInstance().Draw(gameTime);
            CircuitManager.GetInstance().Draw(gameTime);

            ResourcesManager.GetInstance().GetGraphicsDeviceManager().GraphicsDevice.Viewport = v4;
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
