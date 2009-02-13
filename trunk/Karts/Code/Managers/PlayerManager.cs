using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class PlayerManager
    {
        // Constants
        public const int INVALID_PLAYER_ID = 0;
        public const int MAX_LOCAL_PLAYERS = 4;
        
        //---------------------------------------------------
        // Class members
        //---------------------------------------------------
        private List<Player> m_PlayerList = new List<Player>();
        public static PlayerManager m_PlayerManager = null;
        UInt32 m_uIDPlayerCounter;
        public int ActivePlayerIndex { get; set; }

        //---------------------------------------------------
        // Class methods
        //---------------------------------------------------
       public PlayerManager()
       {
           m_uIDPlayerCounter = 0;
           ActivePlayerIndex = -1;
       }

        ~PlayerManager()
        {
            m_PlayerList.Clear();
        }

        public static PlayerManager GetInstance()
        {
            if (m_PlayerManager == null)
                m_PlayerManager = new PlayerManager();

            return m_PlayerManager;
        }

        public bool Init()
        {

            return true;
        }

        public List<Player> GetPlayers()
        {
            return m_PlayerList;
        }

        public int GetNumPlayers()
        {
            return m_PlayerList.Count;
        }

        public int GetNumLocalPlayers()
        {
            int count = 0;
            foreach (Player p in m_PlayerList)
                if (p.Local) count++;
            return count;
        }

        public Player GetPlayerByID(UInt32 uID)
        {
            return m_PlayerList.Find(new FindPlayerID(uID).CompareID);
        }
        
        public Player GetLocalPlayerByIndex(int index)
        {
            return m_PlayerList.Find(new FindPlayerLocalIndex(index).CompareID);
        }

        public bool IsJoinedLocalPlayer(int index)
        {
            return GetLocalPlayerByIndex(index) != null;
        }

        public Player CreatePlayer(){
            // Generate ID
            UInt32 uID = ++m_uIDPlayerCounter;

            Player newPlayer = GetPlayerByID(uID);

            if (newPlayer != null)
            {
                // the id already in use!! Something is wrong!
                return null;
            }

            newPlayer = new Player(uID);
            m_PlayerList.Add(newPlayer);
            return newPlayer;
        }

        public void CreatePlayer(string Name, bool local, bool live)
        {
            Player newPlayer = CreatePlayer();
            newPlayer.Init(Name, local, live, -1, -1);
        }

        public void CreatePlayer(string Name, bool local, bool live, int playerIndex)
        {
            Player newPlayer = CreatePlayer();
            newPlayer.Init(Name, local, live, playerIndex, m_PlayerList.Count-1);
        }

        public bool CreatePlayer(Vector3 position, Vector3 rotation, float fScale, string vehicle_name, string driver_name, bool bCamera)
        {
            Player newPlayer = CreatePlayer();
            return newPlayer.Init(position, rotation, fScale, vehicle_name, driver_name, bCamera);
        }

        public void RemovePlayers() { m_PlayerList.Clear(); }


        public void Update(GameTime GameTime)
        {
            foreach (Player p in m_PlayerList)
                p.Update(GameTime);
	    }

        public void Draw(GameTime GameTime)
        {
            foreach (Player p in m_PlayerList)
                p.Draw(GameTime);
        }

        //---------------------------------------------------
        // Class predicates
        //---------------------------------------------------
        struct FindPlayerID
        {
            UInt32 uID;

            public FindPlayerID(UInt32 _uID) { uID = _uID; }
            public bool CompareID(Player p)
            {
                return p.GetID() == uID;
            }
        }

        struct FindPlayerLocalIndex
        {
            int localIndex;

            public FindPlayerLocalIndex(int _localIndex) { localIndex = _localIndex; }
            public bool CompareID(Player p)
            {
                return p.Local && p.LocalPlayerIndex  == localIndex;
            }
        }
    }
}
