using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

                Debug.Print("Areas positions: "+xValue+" "+zValue);
                ItemArea itemArea = new ItemArea();
                itemArea.Init(new Vector3(xValue, 100.0f, zValue), Vector3.Zero);

                m_ItemAreaList.Add(itemArea);
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
            // Circuit special areas like speeders
        }

        public void Draw(GameTime gameTime)
        {
            Camera ActiveCam = CameraManager.GetInstance().GetActiveCamera();

            foreach (ItemArea itemArea in m_ItemAreaList)
            {
                itemArea.Draw(ActiveCam.GetProjectionMatrix(), ActiveCam.GetViewMatrix());
            }

            m_Mesh.Draw(ActiveCam.GetProjectionMatrix(), ActiveCam.GetViewMatrix());
        }
    }
}
