using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Karts.Code.SceneManager;
using Karts.Code.SceneManager.Components;

using System.Diagnostics;

// ----------------------------------------------------------------------------------
// This class specifies a circuit instance.
// ----------------------------------------------------------------------------------
namespace Karts.Code
{
    class Circuit
    {
        // ------------------------------------------------
        // Class members
        // ------------------------------------------------
        private UInt32 m_uID;                                               // Unique identifier
        private string m_sName;                                             // Name of the circuit
        private Mesh m_Mesh;                                                // 3D Mesh
        private bool m_bReversed;                                           // Do we play it in reverse mode?

        private List<ItemArea> m_ItemAreaList = new List<ItemArea>();       // Item Area
        private List<CheckPoint> m_CheckPointList = new List<CheckPoint>(); // Check points

        TextComponent[] Ranking;

        
        // Type of level
        // Spawn points (for powerups)
        // etc

        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public Circuit() { }
        ~Circuit() { }

        public bool Init(Vector3 position, Vector3 rotation, string model_name)
        {
            m_Mesh = new Mesh();
            m_Mesh.SetPosition(position);
            m_Mesh.SetRotation(rotation);

            Random r = new Random();

            for (int i = 0; i < 10; i++)
            {
                int xValue = r.Next(-1000, 1000);
                int zValue = r.Next(-1000, 1000);

                ItemArea itemArea = new ItemArea();
                itemArea.Init(new Vector3(xValue, 100.0f, zValue), Vector3.Zero);

                m_ItemAreaList.Add(itemArea);
            }

            Vector3 cp_position = Vector3.Zero;

            for (int i = 0; i < 5; i++)
            {
                CheckPoint cp = new CheckPoint();
                cp_position.Z = i * 10000;
                cp.Init(this, null, cp_position, Vector3.Zero, Vector3.UnitZ, false, i);

                m_CheckPointList.Add(cp);
            }

            List<Player> Players = PlayerManager.GetInstance().GetPlayers();
            Ranking = new TextComponent[Players.Count];

            int Y = 30;
            int iCount = 0;
            foreach (Player p in Players)
            {
                Ranking[iCount] = new TextComponent(10, 10 + Y, "", "kartsFont");
                Gui.GetInstance().AddComponent(Ranking[iCount++]);
                Y += 30;
                p.SetLastCheckpointIndex(0);
                m_CheckPointList[0].AddPlayer(p);
            }

            return m_Mesh.Load(model_name);
        }

        public UInt32 GetID()
        {
            return m_uID;
        }

        public void Update(float dt, float t)
        {
            // Manage the items spawn time
            foreach (ItemArea itemArea in m_ItemAreaList)
            {
                itemArea.Update(dt, t);
            }

            // Players check point time?
            UpdateCheckpoints(dt, t);

            
            // Circuit special areas like speeders
        }

        public void OnPlayerForwardCheckpoint(Player p, CheckPoint cp)
        {
            int iTotalCP = m_CheckPointList.Count;
            int iLastPlayerCP = p.GetLastCheckpointIndex();
            int iCurrCP = cp.GetIndex();
            
            int iNumLaps = p.GetLaps();
            
            int iNextCP = (iCurrCP + 1) % iTotalCP;
            int iPrevCP = (iCurrCP - 1) % iTotalCP;
            iPrevCP = iPrevCP < 0 ? (iTotalCP - 1) : iPrevCP;

            bool bGoingForward = (iCurrCP == iLastPlayerCP) || (iCurrCP - iLastPlayerCP) > 0 || (iCurrCP == 0 && iLastPlayerCP == (iTotalCP - 1));

            if (bGoingForward)
            {
                if (iLastPlayerCP == (iTotalCP - 1) && iCurrCP == 0)
                {
                    p.AddLap(1);
                }

                // The player is going on the right direction
                cp.RemovePlayer(p);
                m_CheckPointList[iNextCP].AddPlayer(p);
                p.SetLastCheckpointIndex(iNextCP);
            }
            else
            {/*
                if (iCurrCP != iLastPlayerCP)
                {
                    // The player is going on the wrong direction (backwards)
                    m_CheckPointList[iNextCP].RemovePlayer(p);
                    m_CheckPointList[iCurrCP].AddPlayer(p);
                    p.SetLastCheckpointIndex(iCurrCP);
                }*/
            }
        }

        public void OnPlayerBackwardCheckpoint(Player p, CheckPoint cp)
        {
            int iTotalCP = m_CheckPointList.Count;
            int iLastPlayerCP = p.GetLastCheckpointIndex();
            int iCurrCP = cp.GetIndex();

            if (iCurrCP != iLastPlayerCP)
            {
                int iNextCP = (iCurrCP + 1) % iTotalCP;

                // The player is going on the wrong direction (backwards)
                m_CheckPointList[iNextCP].RemovePlayer(p);
                m_CheckPointList[iCurrCP].AddPlayer(p);
                p.SetLastCheckpointIndex(iCurrCP);
            }
        }
        
        public void UpdateCheckpoints(float dt, float t)
        {
            List<Player> Players = PlayerManager.GetInstance().GetPlayers();
            int iCount = Players.Count;

            int iNumCP = m_CheckPointList.Count;
            for(int j = 0; j < iNumCP; ++j)
            {
                CheckPoint cp = m_CheckPointList[j];
                cp.Update(dt, t);
            }

            int iRankingCount = 0;
            for (int j = (iNumCP-1); j >= 0; --j)
            {
                CheckPoint cp = m_CheckPointList[j];

                List<Player> Rankings = cp.GetRankings();
                int iNumRanks = Rankings.Count;

                for (int i = 0; i < iNumRanks; ++i)
                {
                    Player p = Rankings[i];

                    if (p != null)
                        Ranking[iRankingCount++].Text = iRankingCount + " position: " + p.GetName();
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            Camera ActiveCam = CameraManager.GetInstance().GetActiveCamera();

            foreach (ItemArea itemArea in m_ItemAreaList)
            {
                itemArea.Draw(ActiveCam.GetProjectionMatrix(), ActiveCam.GetViewMatrix());
            }

            foreach (CheckPoint cp in m_CheckPointList)
            {
                cp.Draw();
            }

            m_Mesh.Draw(ActiveCam.GetProjectionMatrix(), ActiveCam.GetViewMatrix());
        }
    }
}
