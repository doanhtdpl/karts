using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karts.Code
{
    class CollisionManager
    {
        public static CollisionManager m_CollisionManager = null;
        private List<Area> m_Areas = new List<Area>();
        private float m_fUpdateTime = 0.0f;
        private static float MAX_UPDATE_TIME = 0.1f;

        public static CollisionManager GetInstance()
        {
            if (m_CollisionManager == null)
                m_CollisionManager = new CollisionManager();

            return m_CollisionManager;
        }

        bool CheckCollision(ref Mesh m1, ref Mesh m2)
        {
            BoundingSphere c1BoundingSphere = m1.GetBoundingSphere();
            BoundingSphere c2BoundingSphere = m2.GetBoundingSphere();
    
            if (c1BoundingSphere.Intersects(c2BoundingSphere))
            {
                return true;
            }

            return false;
        }

        public Area CreateArea(Vector3 pos, Vector3 rot, float fWidth, float fHeight, float fDepth, int iLife)
        {
            Area newArea = new Area();
            if (newArea.Init(pos, rot, fWidth, fHeight, fDepth, iLife))
            {
                m_Areas.Add(newArea);

                return newArea;
            }

            return null;
        }

        public void Update(float dt, float t)
        {
            if (m_fUpdateTime + MAX_UPDATE_TIME > t) 
                return;

            m_fUpdateTime = t;

            // We manage the collision between players and areas
            List<Player> Players = PlayerManager.GetInstance().GetPlayers();
            int jCount = Players.Count;
            for (int j = 0; j < jCount; ++j)
            {
                Player p = Players[j];
                int iCount = m_Areas.Count;
                for (int i = 0; i < iCount; ++i)
                {
                    Area a = m_Areas[i];
                    //float fSqDist = (p.GetPosition() - a.GetPosition()).LengthSquared();
                    OBB obb = a.GetOBB();

                    if (/*fSqDist < 30000000f && */obb.Contains(p.GetVehicle().GetBoundingSphere()) != ContainmentType.Disjoint)
                    {
                        // If we are near the area and the mesh is not out of it we call on enter
                        a.OnEnter(p.GetVehicle());
                        
                        // Note: The OnExit action is calculated inside each area.
                    }
                }
            }

            foreach (Area a in m_Areas)
            {
                a.Update();
            }
        }
    }    
}
