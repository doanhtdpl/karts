using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class PlayerManager : DrawableGameComponent
    {
        // Constants
        public const int INVALID_PLAYER_ID = 0;

        //---------------------------------------------------
        // Class members
        //---------------------------------------------------
        private List<Player> m_PlayerList = new List<Player>();
        public static PlayerManager m_PlayerManager = null;
        UInt32 m_uIDPlayerCounter;

        //---------------------------------------------------
        // Class methods
        //---------------------------------------------------
       public PlayerManager(Game game) : base(game) 
        {
            m_uIDPlayerCounter = 0;
        }

        ~PlayerManager()
        {
            m_PlayerList.Clear();
        }

        public static PlayerManager Init(Game game)
        {
            if (m_PlayerManager == null)
                m_PlayerManager = new PlayerManager(game);

            return m_PlayerManager;
        }

        public static PlayerManager GetInstance()
        {
            return m_PlayerManager;
        }

        public List<Player> GetPlayers()
        {
            return m_PlayerList;
        }

        public int GetNumPlayers()
        {
            return m_PlayerList.Count;
        }

        public Player GetPlayerByID(UInt32 uID)
        {
            return m_PlayerList.Find(new FindPlayerID(uID).CompareID);
        }

        public bool CreatePlayer(Vector3 position, Vector3 rotation, float fScale, string Name, string vehicle_name, string driver_name, bool bCamera)
        {
            // Generate ID
            UInt32 uID = ++m_uIDPlayerCounter;

            Player newPlayer = GetPlayerByID(uID);

            if (newPlayer != null)
            {
                // the id already in use!! Something is wrong!
                return false;
            }

            newPlayer = new Player();
            if (newPlayer.Init(position, rotation, fScale, Name, uID, vehicle_name, driver_name, bCamera))
            {
                m_PlayerList.Add(newPlayer);
            }

            return true;
        }

        public void RemovePlayers() { m_PlayerList.Clear(); }


        public override void Update(GameTime GameTime)
        {
            foreach (Player p in m_PlayerList)
                p.Update(GameTime);
	    }

        public override void Draw(GameTime GameTime)
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
    }
}
