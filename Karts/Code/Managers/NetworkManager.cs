using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Net;

namespace Karts.Code
{
    public class NetworkManager
    {
        public static int MAX_LOCAL_PLAYERS = 4;
        public static int MAX_TOTAL_PLAYERS = 31;

        private NetworkSession session = null;

        public static NetworkManager m_NetworkManager = null;

        public static NetworkManager GetInstance()
        {
            if (m_NetworkManager == null)
                m_NetworkManager = new NetworkManager();

            return m_NetworkManager;
        }

        public void CreateSession()
        {
            session = NetworkSession.Create(NetworkSessionType.SystemLink, MAX_LOCAL_PLAYERS, MAX_TOTAL_PLAYERS);

            session.AllowHostMigration = true;
            session.AllowJoinInProgress = true;

            session.GamerJoined += new EventHandler<GamerJoinedEventArgs>(session_GamerJoined);
            session.GamerLeft += new EventHandler<GamerLeftEventArgs>(session_GamerLeft);
            session.GameStarted += new EventHandler<GameStartedEventArgs>(session_GameStarted);
            session.GameEnded += new EventHandler<GameEndedEventArgs>(session_GameEnded);
            session.SessionEnded += new EventHandler<NetworkSessionEndedEventArgs>(session_SessionEnded);
        }

        public void DestroySession()
        {
            if (session != null)
            {
                if (session.AllGamers.Count == 1)
                {
                    session.EndGame();
                    session.Update();
                }
            }
        }

        public void Update()
        {
            if(session != null){
                session.Update();
            }
        }

        void session_GamerJoined(object sender, GamerJoinedEventArgs p) { }
        void session_GamerLeft(object sender, GamerLeftEventArgs p) { }
        void session_GameStarted(object sender, GameStartedEventArgs p) { }
        void session_GameEnded(object sender, GameEndedEventArgs p) { }
        void session_SessionEnded(object sender, NetworkSessionEndedEventArgs p) { }
    }
}
