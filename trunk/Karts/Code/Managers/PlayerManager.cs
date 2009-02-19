using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Karts.Code.SceneManager;
using Karts.Code.SceneManager.Components;

using System.Diagnostics;

namespace Karts.Code
{
    class PlayerManager
    {
        // Constants
        public const int MAX_LOCAL_PLAYERS = 4;
        
        //---------------------------------------------------
        // Class members
        //---------------------------------------------------
        private List<Player> m_PlayerList = new List<Player>();
        public static PlayerManager m_PlayerManager = null;
        UInt32 m_uIDPlayerCounter;
        public int ActivePlayerIndex { get; set; }

        private List<TextComponent> m_Ranking = new List<TextComponent>();

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

        public Player GetPlayerByVehicleMesh(Mesh m)
        {
            return m_PlayerList.Find(new FindPlayerVehicleMesh(m).CompareMesh);
        }
        
        public Player GetLocalPlayerByIndex(int index)
        {
            return m_PlayerList.Find(new FindPlayerLocalIndex(index).CompareID);
        }

        public bool IsJoinedLocalPlayer(int index)
        {
            return GetLocalPlayerByIndex(index) != null;
        }

        public Player CreatePlayer()
        {
            // Generate ID
            UInt32 uID = ++m_uIDPlayerCounter;

            return CreatePlayer(uID);
        }

        public Player CreatePlayer(UInt32 uID)
        {
            Player newPlayer = GetPlayerByID(uID);

            if (newPlayer != null)
            {
                // the id already in use!! Something is wrong!
                return null;
            }

            newPlayer = new Player();
            m_PlayerList.Add(newPlayer);

            TextComponent rank = new TextComponent(10, 10 + 30*(m_Ranking.Count+1), "", "kartsFont");
            m_Ranking.Add(rank);
            Gui.GetInstance().AddComponent(rank);

            return newPlayer;
        }

        public Player CreatePlayer(string Name, bool local, bool live)
        {
            Player newPlayer = CreatePlayer();
            newPlayer.Init(Name, local, live, -1, -1);
            return newPlayer;
        }

        public Player CreatePlayer(string Name, bool local, bool live, UInt32 uID)
        {
            Player newPlayer = CreatePlayer(uID);
            newPlayer.Init(Name, local, live, -1, -1);
            return newPlayer;
        }

        public Player CreatePlayer(string Name, bool local, bool live, int playerIndex)
        {
            Player newPlayer = CreatePlayer();
            newPlayer.Init(Name, local, live, playerIndex, m_PlayerList.Count-1);
            return newPlayer;
        }

        public bool CreatePlayer(Vector3 position, Vector3 rotation, float fScale, string vehicle_name, string driver_name, bool bCamera)
        {
            Player newPlayer = CreatePlayer();
            return newPlayer.Init(position, rotation, fScale, vehicle_name, driver_name, bCamera);
        }

        public void RemovePlayers() { m_PlayerList.Clear(); }


        public void Update(GameTime GameTime)
        {
            // We Order the players depending on its position
            if (m_PlayerList.Count > 1)
                m_PlayerList.Sort(new CompareRanking().Compare);

            int iCount= 0;
            foreach (Player p in m_PlayerList)
            {
                p.Update(GameTime);
                m_Ranking[iCount++].Text = iCount + " position: " + p.GetName();

            }
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

        struct FindPlayerVehicleMesh
        {
            Mesh m;

            public FindPlayerVehicleMesh(Mesh _m) { m = _m; }
            public bool CompareMesh(Player p)
            {
                return (Mesh)(p.GetVehicle()) == m;
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

        public struct CompareRanking : IComparable
        {
            public int CompareTo(object otro)
            {
                return 1;
            }

            public int Compare(Player p1, Player p2)
            {
                if (p1 == null || p2 == null || p1 == p2)
                    return 0;

                Player.CircuitState state1 = p1.GetCircuitState();
                Player.CircuitState state2 = p2.GetCircuitState();

                //Debug.Print("Laps " + state1.iLaps + ", " + state2.iLaps);
                if (state1.iLaps > state2.iLaps)
                {
                    //Debug.Print("Laps: Player " + p1.GetName() + " before than " + p2.GetName());
                    return -1;
                }
                else if (state1.iLaps == state2.iLaps)
                {
                    //Debug.Print("CP: " + state1.iCheckPoint + ", " + state2.iCheckPoint);
                    if (state1.iCheckPoint > state2.iCheckPoint)
                    {
                        //Debug.Print("CP: Player " + p1.GetName() + " before than " + p2.GetName());
                        return -1;
                    }
                    else if (state1.iCheckPoint == state2.iCheckPoint)
                    {
                        //Debug.Print("Dist: " + state1.fSqCheckpointDist + ", " + state2.fSqCheckpointDist);
                        if (state1.fSqCheckpointDist <= state2.fSqCheckpointDist)
                        {
                            //Debug.Print("Dist: Player " + p1.GetName() + " before than " + p2.GetName());
                            return -1;
                        }
                    }
                }

                //Debug.Print("Player " + p2.GetName() + " before than " + p1.GetName());
                return 1;
            }
        }  
    }
}
