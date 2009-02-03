using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karts.Code
{
    class FindMultiplayerGame : GameState
    {
        public override EGameStateType GetStateType() { return EGameStateType.EGM_FIND_MULTIPLAYER_GAME; }

        private AvailableNetworkSessionCollection availableSessions;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        public override void Enter()
        {
            availableSessions = NetworkManager.GetInstance().GetAvailableSessions();
            spriteBatch = Karts.spriteBatch;
            spriteFont = ResourcesManager.GetInstance().GetContentManager().Load<SpriteFont>("KartsFont");
        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {
            AvailableNetworkSession availableSession;
            spriteBatch.Begin();
            for (int i = 0; i < availableSessions.Count; ++i)
            {
                availableSession = availableSessions[i];

                string HostGamerTag = availableSession.HostGamertag;
                int GamersInSession = availableSession.CurrentGamerCount;
                int OpenPrivateGamerSlots = availableSession.OpenPrivateGamerSlots;
                int OpenPublicGamerSlots = availableSession.OpenPublicGamerSlots;
                string sessionInformation = "Session available from gamertag " + HostGamerTag +
                    "\n" + GamersInSession + " players already in this session. \n" +
                    +OpenPrivateGamerSlots + " open private player slots available. \n" +
                    +OpenPublicGamerSlots + " public player slots available.";

                spriteBatch.DrawString(spriteFont, sessionInformation, new Vector2(100, 100 + i*100), Color.Black);
            }
            spriteBatch.End();

            base.Draw(GameTime);
        }

        public override void Exit()
        {
        }
    }
}
