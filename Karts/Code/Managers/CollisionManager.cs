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

        public static CollisionManager GetInstance()
        {
            if (m_CollisionManager == null)
                m_CollisionManager = new CollisionManager();

            return m_CollisionManager;
        }

        bool CheckCollision(ref Mesh m1, ref Mesh m2)
        {
            for (int i = 0; i < m1.GetModel().Meshes.Count; i++)
            {
                // Check whether the bounding boxes of the two cubes intersect.
                BoundingSphere c1BoundingSphere = m1.GetModel().Meshes[i].BoundingSphere;
                c1BoundingSphere.Center += m1.GetPosition();

                for (int j = 0; j < m2.GetModel().Meshes.Count; j++)
                {
                    BoundingSphere c2BoundingSphere = m2.GetModel().Meshes[j].BoundingSphere;
                    c2BoundingSphere.Center += m2.GetPosition();

                    if (c1BoundingSphere.Intersects(c2BoundingSphere))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }    
}
