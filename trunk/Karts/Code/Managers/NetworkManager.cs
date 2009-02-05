using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.GamerServices;

namespace Karts.Code
{
    public class NetworkManager : GameComponent
    {
        public static int MAX_LOCAL_PLAYERS = 4;
        public static int MAX_TOTAL_PLAYERS = 16;

        private NetworkSession session = null;
        private AvailableNetworkSessionCollection availableSessions = null;

        public static NetworkManager m_NetworkManager = null;

        public static NetworkManager GetInstance()
        {
            return m_NetworkManager;
        }

        public static NetworkManager Init(Game game)
        {
            if (m_NetworkManager == null)
                m_NetworkManager = new NetworkManager(game);

            return m_NetworkManager;
        }

        private NetworkManager(Game game)
            : base(game)
        {
        }

        public NetworkSession CreateSession()
        {
            session = NetworkSession.Create(NetworkSessionType.SystemLink, MAX_LOCAL_PLAYERS, MAX_TOTAL_PLAYERS);

            session.AllowHostMigration = true;
            session.AllowJoinInProgress = true;

            session.GamerJoined += new EventHandler<GamerJoinedEventArgs>(session_GamerJoined);
            session.GamerLeft += new EventHandler<GamerLeftEventArgs>(session_GamerLeft);
            session.GameStarted += new EventHandler<GameStartedEventArgs>(session_GameStarted);
            session.GameEnded += new EventHandler<GameEndedEventArgs>(session_GameEnded);
            session.SessionEnded += new EventHandler<NetworkSessionEndedEventArgs>(session_SessionEnded);

            return session;
        }

        public NetworkSession GetSession()
        {
            return session;
        }

        public void JoinSession(AvailableNetworkSession hostSession)
        {
            session = NetworkSession.Join(hostSession);
        }

        public void LeaveSession()
        {
            session.Dispose();
        }

        public AvailableNetworkSessionCollection FindSessions()
        {
            availableSessions = NetworkSession.Find(NetworkSessionType.SystemLink, MAX_LOCAL_PLAYERS, null);
            return availableSessions;
        }

        public AvailableNetworkSessionCollection GetAvailableSessions()
        {
            if (availableSessions == null)
            {
                availableSessions = FindSessions();
            }
            return availableSessions;
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

        void session_GamerJoined(object sender, GamerJoinedEventArgs p) { 
        }

        void session_GamerLeft(object sender, GamerLeftEventArgs p) { 
        }

        void session_GameStarted(object sender, GameStartedEventArgs p) { 
        }

        void session_GameEnded(object sender, GameEndedEventArgs p) { 
        }

        void session_SessionEnded(object sender, NetworkSessionEndedEventArgs p) { 
        }
    }
}
