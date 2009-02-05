using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Karts.Code.SceneManager;
using Karts.Code.SceneManager.Components;

namespace Karts.Code
{
    class FindMultiplayerGame : GameState
    {
        private AvailableNetworkSessionCollection availableSessions;

        private int selected = 0;

        private Screen menu;

        public override void Enter()
        {
            menu = new Screen();
            Gui.GetInstance().AddComponent(menu);

            UpdateSessions();
        }

        public override void Update(GameTime GameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if(state.IsKeyDown(Keys.F5)){
                UpdateSessions();
            }else if(state.IsKeyDown(Keys.Down)){
                selected = (selected + availableSessions.Count + 1) % availableSessions.Count;
            }else if(state.IsKeyDown(Keys.Up)){
                selected = (selected + availableSessions.Count - 1) % availableSessions.Count;
            }
            else if (state.IsKeyDown(Keys.Enter))
            {
                NetworkManager.GetInstance().JoinSession(availableSessions[selected]);
                GameStateManager.GetInstance().ChangeState(new WaitForOtherPlayers());
            }
            else if (state.IsKeyDown(Keys.Back))
            {
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

        private void UpdateSessions()
        {
            menu.RemoveAll();

            availableSessions = NetworkManager.GetInstance().GetAvailableSessions();

            AvailableNetworkSession availableSession;
            for (int i = 0; i < availableSessions.Count; ++i)
            {
                availableSession = availableSessions[i];

                string HostGamerTag = availableSession.HostGamertag;
                int GamersInSession = availableSession.CurrentGamerCount;
                int OpenPrivateGamerSlots = availableSession.OpenPrivateGamerSlots;
                int OpenPublicGamerSlots = availableSession.OpenPublicGamerSlots;
                string sessionInformation = HostGamerTag + "("+ GamersInSession + "/" + OpenPrivateGamerSlots + "/" + OpenPublicGamerSlots + ")";

                menu.AddComponent(new TextComponent(100, 100 * (i+1), sessionInformation, "KartsFont"));
            }
        }
    }
}
