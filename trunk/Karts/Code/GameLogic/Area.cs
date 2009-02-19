using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Karts.Code
{
    interface IObserver
    {
        void OnEnter(Mesh m);
        void OnExit(Mesh m);
    }
    //---------------------------------------
    //---------------------------------------
    // Class Area:
    //
    // This class will define an area trigger.
    //---------------------------------------
    //---------------------------------------
    class Area : Object3D
    {
        //---------------------------------------
        // Class members
        //---------------------------------------
        private OBB m_obb;
        private int m_iLife;      // < 0 -> Infinite life
        private List<Mesh> m_MeshesInside = new List<Mesh>();
        private List<Mesh> m_MeshesToDelete = new List<Mesh>();
        private List<IObserver> m_Observers = new List<IObserver>();
        private Vector3 m_vSize;
        private Color m_Color = Color.Red;

        //---------------------------------------
        // Class methods
        //---------------------------------------
        public bool Init(Vector3 pos, Vector3 rot, float fWidth, float fHeight, float fDepth, int iLife, Color c)
        {
            m_vPosition = pos; // Base position
            pos.Y += fHeight;
            m_vRotation = rot;
            m_iLife = iLife;
            m_vSize = new Vector3(fWidth, fHeight, fDepth);
            m_Color = c;


            m_obb = new OBB(pos, new Vector3(fWidth, fHeight, fDepth), Matrix.CreateFromYawPitchRoll(rot.Y, rot.X, rot.Z));

            return true;
        }

        public bool IsAlife()
        {
            return m_iLife > 0 || m_iLife < 0;
        }

        public void RegisterObserver(IObserver obs)
        {
            if (!m_Observers.Contains(obs))
            {
                m_Observers.Add(obs);
            }
        }

        public void UnregisterObserver(IObserver obs)
        {
            if (m_Observers.Contains(obs))
            {
                m_Observers.Remove(obs);
            }
        }

        public void OnEnter(Mesh m)
        {
            --m_iLife;

            if (!m_MeshesInside.Contains(m))
            {
                Debug.Print("==============> Entering trigger");
                m_MeshesInside.Add(m);

                foreach (IObserver o in m_Observers)
                {
                    o.OnEnter(m);
                }
            }
        }

        public void OnExit(Mesh m)
        {
            Debug.Print("==============> Exiting trigger");
            foreach (IObserver o in m_Observers)
            {
                o.OnExit(m);
            }
        }

        public bool IsInside(Mesh m)
        {
            return m_MeshesInside.Contains(m);
        }

        public void Update()
        {
            int iCount = m_MeshesInside.Count;
            for (int i = 0; i < iCount; ++i)
            {
                Mesh m = m_MeshesInside[i];
                if (m_obb.Contains(m.GetBoundingSphere()) == ContainmentType.Disjoint)
                {
                    // The mesh has exited the area
                    OnExit(m);
                    m_MeshesToDelete.Add(m);
                }
            }

            // Delete exited meshes
            iCount = m_MeshesToDelete.Count;
            for (int i = 0; i < iCount; i++)
            {
                m_MeshesInside.Remove(m_MeshesToDelete[i]);
                m_MeshesToDelete[i] = null;
            }

            m_MeshesToDelete.Clear();
        }

        public OBB GetOBB()
        {
            return m_obb;
        }

        // Just for Debug
        public void Draw()
        {
            m_obb.Draw(m_Color);
        }
    }
}
