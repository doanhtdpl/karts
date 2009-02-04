using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Karts.Code
{
    class FindMultiplayerGame : GameState
    {
        private AvailableNetworkSessionCollection availableSessions;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        private int selected = 0;

        public override void Enter()
        {
            availableSessions = NetworkManager.GetInstance().GetAvailableSessions();
            spriteBatch = Karts.spriteBatch;
            spriteFont = ResourcesManager.GetInstance().GetContentManager().Load<SpriteFont>("KartsFont");
        }

        public override void Update(GameTime GameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if(state.IsKeyDown(Keys.F5)){
                selected = 0;
                availableSessions = NetworkManager.GetInstance().GetAvailableSessions();
            }else if(state.IsKeyDown(Keys.Down)){
                selected = (selected + availableSessions.Count + 1) % availableSessions.Count;
            }else if(state.IsKeyDown(Keys.Up)){
                selected = (selected + availableSessions.Count - 1) % availableSessions.Count;
            }else if (state.IsKeyDown(Keys.Enter)){
                NetworkManager.GetInstance().JoinSession(availableSessions[selected]);
                GameStateManager.GetInstance().ChangeState(new WaitForOtherPlayers());
            }

            base.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {
            AvailableNetworkSession availableSession;
            for (int i = 0; i < availableSessions.Count; ++i)
            {
                availableSession = availableSessions[i];

                string HostGamerTag = availableSession.HostGamertag;
                int GamersInSession = availableSession.CurrentGamerCount;
                int OpenPrivateGamerSlots = availableSession.OpenPrivateGamerSlots;
                int OpenPublicGamerSlots = availableSession.OpenPublicGamerSlots;
                string sessionInformation = "Session available from gamertag " + HostGamerTag +
                    "\n\t" + GamersInSession + " players already in this session. \n\t" +
                    +OpenPrivateGamerSlots + " open private player slots available. \n\t" +
                    +OpenPublicGamerSlots + " public player slots available.";

                DrawDebugManager.GetInstance().DrawText(sessionInformation, 100, 100 + i * 100, Color.Black);
            }

            base.Draw(GameTime);
        }

        public override void Exit()
        {
        }
    }
}
