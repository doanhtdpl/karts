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

        public const byte N_JOIN_PLAYER = 0;
        public const byte N_ACCEPT_JOIN = 1;
        public const byte N_PLAYER_JOINED = 2;
        public const byte N_PUBLIC_CHAT_MESSAGE = 3;
        public const byte N_PRIVATE_CHAT_MESSAGE = 4;
        public const byte N_PLAYER_LEFT = 5;
        public const byte N_PLAYER_INFO = 6;

        private NetworkSession session = null;
        private AvailableNetworkSessionCollection availableSessions = null;
        private PacketWriter pw;
        private PacketReader pr;
        private LocalNetworkGamer sender;

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

            pr = new PacketReader();
            pw = new PacketWriter();

            sender = session.LocalGamers[0];

            return session;
        }

        public bool HasSession()
        {
            return session != null;
        }

        public bool IsClient()
        {
            return session != null && !session.IsHost;
        }

        public NetworkSession GetSession()
        {
            return session;
        }

        public void JoinSession(AvailableNetworkSession hostSession)
        {
            session = NetworkSession.Join(hostSession);

            sender = session.LocalGamers[0];
        }

        public void LeaveSession()
        {
            if (HasSession())
            {
                session.Dispose();
                session = null;
            }
        }

        public AvailableNetworkSessionCollection FindSessions()
        {
            availableSessions = NetworkSession.Find(NetworkSessionType.SystemLink, MAX_LOCAL_PLAYERS, null);
            return availableSessions;
        }

        public AvailableNetworkSessionCollection GetAvailableSessions()
        {
            /*if (availableSessions == null)
            {
                availableSessions = FindSessions();
            }
             */
            availableSessions = FindSessions();
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

            if (sender.IsDataAvailable)
            {
                NetworkGamer messageSender;
                sender.ReceiveData(pr, out messageSender);

                byte id = pr.ReadByte();
                switch (id)
                {
                    case N_JOIN_PLAYER:
                        String name = pr.ReadString();

                        Player newPlayer = PlayerManager.GetInstance().CreatePlayer(name, false, false);

                        pw.Write(N_ACCEPT_JOIN);
                        pw.Write(newPlayer.GetID());
                        pw.Write(name);
                        sender.SendData(pw, SendDataOptions.None, messageSender);
                        break;
                    case N_ACCEPT_JOIN:
                        UInt32 playerID = pr.ReadUInt32();
                        String playerName = pr.ReadString();
                        PlayerManager.GetInstance().CreatePlayer(playerName, true, false, playerID);

                        CommunicatePlayerJoined(playerName, playerID);
                        break;
                    case N_PLAYER_JOINED:
                        playerID = pr.ReadUInt32();
                        playerName = pr.ReadString();
                        PlayerManager.GetInstance().CreatePlayer(playerName, false, false, playerID);
                        break;
                    case N_PLAYER_LEFT:
                        break;
                    case N_PLAYER_INFO:
                        break;
                    case N_PRIVATE_CHAT_MESSAGE:
                        break;
                    case N_PUBLIC_CHAT_MESSAGE:
                        break;
                }
            }
        }

        public void JoinPlayer(String playerName)
        {
            pw.Write(N_JOIN_PLAYER);
            pw.Write(playerName);
            sender.SendData(pw, SendDataOptions.None, session.Host);
        }

        public void CommunicatePlayerJoined(String playerName, UInt32 uID)
        {
            pw.Write(N_PLAYER_JOINED);
            pw.Write(uID);
            pw.Write(playerName);
            sender.SendData(pw, SendDataOptions.None, session.Host);
        }

        void session_GamerJoined(object sender, GamerJoinedEventArgs p)
        { 
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
