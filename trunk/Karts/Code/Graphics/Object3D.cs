using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karts.Code
{
    class Object3D
    {
        //--------------------------------------------
        // Class members
        //--------------------------------------------
        protected Vector3 m_vPosition;
        protected Vector3 m_vRotation; // Pitch (rotation in X axis), Yaw (rotation in Y axis), Roll (rotation in Z axis)

        // debug
        private bool m_bDrawAxis;


        //--------------------------------------------
        // Class methods
        //--------------------------------------------
        public Object3D() 
        {
            m_vPosition = Vector3.Zero;
            m_vRotation = Vector3.Zero;
            m_bDrawAxis = true;
        }

        ~Object3D() { }

        public Vector3 GetPosition()
        {
            return m_vPosition;
        }

        public void SetPosition(float x, float y, float z)
        {
            m_vPosition.X = x;
            m_vPosition.Y = y;
            m_vPosition.Z = z;
        }

        public void SetPosition(Vector3 pos)
        {
            m_vPosition = pos;
        }

        public void AddPosition(Vector3 pos)
        {
            m_vPosition = m_vPosition + pos;
        }

        public void AddPosition(float x, float y, float z)
        {
            m_vPosition = m_vPosition + new Vector3(x, y, z);
        }

        public Vector3 GetForward()
        {
            Matrix m = Matrix.CreateFromYawPitchRoll(m_vRotation.Y, m_vRotation.X, m_vRotation.Z);
            
            Vector3 vDir = m.Forward;
            vDir.Normalize();
            return vDir;
        }

        public Vector3 GetRight()
        {
            Matrix m = Matrix.CreateFromYawPitchRoll(m_vRotation.Y, m_vRotation.X, m_vRotation.Z);
            Vector3 vRight = m.Right;
            vRight.Normalize();
            return vRight;
        }

        public Vector3 GetUp()
        {
            Matrix m = Matrix.CreateFromYawPitchRoll(m_vRotation.Y, m_vRotation.X, m_vRotation.Z);
            Vector3 vUp = m.Up;
            vUp.Normalize();
            return vUp;
        }

        public Vector3 GetRotation()
        {
            return m_vRotation;
        }

        public void SetRotation(float fYaw, float fPitch, float fRoll)
        {
            m_vRotation.X = fPitch;
            m_vRotation.Y = fYaw;
            m_vRotation.Z = fRoll;
        }
        
        public void SetRotation(Vector3 rotation)
        {
            m_vRotation = rotation;
        }

        public void Draw(Matrix camProjMatrix, Matrix camViewMatrix)
        {
            if (m_bDrawAxis)
            {
                Vector3 fwd = GetForward();
                Vector3 up = GetUp();
                Vector3 right = GetRight();

                DrawDebugManager.GetInstance().DrawLine(m_vPosition, m_vPosition + fwd * 5000, Color.Blue);
                DrawDebugManager.GetInstance().DrawLine(m_vPosition, m_vPosition + up* 5000, Color.Green);
                DrawDebugManager.GetInstance().DrawLine(m_vPosition, m_vPosition + right * 5000, Color.Red);
            }
        }

        //--------------------------------------------------------
        // Debug functions
        //--------------------------------------------------------
        public void DrawAxis(bool draw)
        {
            m_bDrawAxis = draw;
        }
    }
}
