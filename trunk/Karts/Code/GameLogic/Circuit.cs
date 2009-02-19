using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
//using Karts.Code.SceneManager;
//using Karts.Code.SceneManager.Components;

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
            float rot = 0.0f;

            for (int i = 0; i < 8; i++)
            {
                cp_position = new Vector3(30000, 0, 0);
                rot += MathHelper.TwoPi / 8;

                CheckPoint cp = new CheckPoint();
                cp_position = Vector3.Transform(cp_position, Matrix.CreateFromYawPitchRoll(rot, 0, 0));
                cp.Init(this, null, cp_position, new Vector3(0.0f, rot, 0.0f), false, i);

                m_CheckPointList.Add(cp);
            }

            return m_Mesh.Load(model_name);
        }

        public CheckPoint GetCheckpoint(int idx)
        {
            if (idx >= 0 && idx < m_CheckPointList.Count)
                return m_CheckPointList[idx];

            return null;
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
            Player.CircuitState PlayerState = p.GetCircuitState();

            int iTotalCP = m_CheckPointList.Count;
            int iLastPlayerCP = PlayerState.iCheckPoint;
            int iCurrCP = cp.GetIndex();

            int iNumLaps = PlayerState.iLaps;

            if (iLastPlayerCP == 0 && iCurrCP == 0)
            {
                PlayerState.iLaps += 1;
            }

            iLastPlayerCP = iLastPlayerCP < 0 ? 0 : iLastPlayerCP;

            int iNextCP = (iLastPlayerCP + 1) % iTotalCP;
            int iPrevCP = (iLastPlayerCP - 1) % iTotalCP;
            iPrevCP = iPrevCP < 0 ? (iTotalCP - 1) : iPrevCP;

            // The player is going on the right direction
            PlayerState.iCheckPoint = iNextCP;
        }

        public void OnPlayerBackwardCheckpoint(Player p, CheckPoint cp)
        {
            Player.CircuitState PlayerState = p.GetCircuitState();

            int iTotalCP = m_CheckPointList.Count;
            int iLastPlayerCP = PlayerState.iCheckPoint;
            int iCurrCP = cp.GetIndex();

            if (iCurrCP != iLastPlayerCP)
            {
                int iNextCP = (iCurrCP + 1) % iTotalCP;

                // The player is going on the wrong direction (backwards)
                PlayerState.iCheckPoint = iCurrCP;
            }
        }
        
        public void UpdateCheckpoints(float dt, float t)
        {
            int iNumCP = m_CheckPointList.Count;
            for(int j = 0; j < iNumCP; ++j)
            {
                CheckPoint cp = m_CheckPointList[j];
                cp.Update(dt, t);
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
