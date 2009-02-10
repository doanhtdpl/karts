using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
        private UInt32 m_uID;           // Unique identifier
        private Mesh m_Mesh;    // 3D Mesh

        // Powerups of the level
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

            return m_Mesh.Load(model_name);
        }

        public UInt32 GetID()
        {
            return m_uID;
        }

        public void Update(GameTime gameTime)
        {
            // Manage the items spawn time
            // Players check point time?
            // Circuit special areas like speeders
        }

        public void Draw(GameTime gameTime)
        {
            Camera ActiveCam = CameraManager.GetInstance().GetActiveCamera();
            m_Mesh.Draw(ActiveCam.GetProjectionMatrix(), ActiveCam.GetViewMatrix());
        }
    }
}
