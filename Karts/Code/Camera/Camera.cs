using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class Camera
    {
        //---------------------------------------
        // Class types
        //---------------------------------------
        public enum ECamType
        {
            ECAMERA_TYPE_INVALID = 0,
            ECAMERA_TYPE_TARGET = 1,
            ECAMERA_TYPE_FPS = 2,
            ECAMERA_TYPE_FREE = 3
        };

        //---------------------------------------
        // Class members
        //---------------------------------------
        // Camera properties
        protected int m_iIDCamera;

        protected Vector3 m_vLookAt;
        protected Vector3 m_vPosition;
        protected Vector3 m_vUp;

        // Perspective
        protected float m_fAspectRatio;
        protected float m_fFieldOfView;
        protected float m_fNearPlaneDistance;
        protected float m_fFarPlaneDistance;
        protected Matrix m_ViewMatrix;
        protected Matrix m_ProjMatrix;


        //---------------------------------------
        // Class methods
        //---------------------------------------
        public Camera()
        {
            m_vLookAt = Vector3.Zero;
            m_vPosition = Vector3.Zero;
            m_vUp = Vector3.Up;
            
            m_iIDCamera = -1;

            m_fAspectRatio = 4.0f / 3.0f;
            m_fFieldOfView = MathHelper.ToRadians(45.0f);
            m_fNearPlaneDistance = 1.0f;
            m_fFarPlaneDistance = 10000.0f;
        }

        ~Camera() { }

        public bool Init(int ID)
        {
            m_iIDCamera = ID;

            m_vPosition = new Vector3(0.0f, 100.0f, -100.0f);
            m_vLookAt = new Vector3(0.0f, 0.0f, 100.0f);            

            // Set camera perspective
            m_fNearPlaneDistance = 1.0f;
            m_fFarPlaneDistance = 100000.0f;

            GraphicsDeviceManager gdm = ResourcesManager.GetInstance().GetGraphicsDeviceManager();
            m_fAspectRatio = (float)gdm.GraphicsDevice.Viewport.Width / gdm.GraphicsDevice.Viewport.Height;

            return true;
        }

        public new ECamType GetType() { return ECamType.ECAMERA_TYPE_INVALID; }

        public void SetID(int id)
        {
            if (id < 0) return;

            m_iIDCamera = id;
        }

        public int GetID()
        {
            return m_iIDCamera;
        }


        public Vector3 GetLookAt()
        {
            return m_vLookAt;
        }

        public Vector3 GetForward()
        {
            return m_ViewMatrix.Forward;
        }

        public Vector3 GetUp()
        {
            return m_ViewMatrix.Up;
        }

        public Vector3 GetRight()
        {
            return m_ViewMatrix.Right;
        }

        public Vector3 GetPosition()
        {
            return m_vPosition;
        }

        public Matrix GetProjectionMatrix()
        {
            return m_ProjMatrix;
        }

        public Matrix GetViewMatrix()
        {
            return m_ViewMatrix;
        }

        protected void UpdateMatrices()
        {
            m_ViewMatrix = Matrix.CreateLookAt(m_vPosition, m_vLookAt, m_vUp);
            m_ProjMatrix = Matrix.CreatePerspectiveFieldOfView(m_fFieldOfView, m_fAspectRatio, m_fNearPlaneDistance, m_fFarPlaneDistance);
        }

        public virtual void Update(GameTime gameTime) { }
    }
}
