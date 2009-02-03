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
        // Class members
        //---------------------------------------
        // Camera properties
        private Object3D m_Target;
        
        private Vector3 m_vLookAt;
        private Vector3 m_vLookAtOffset;
        
        private Vector3 m_vPosition;
        private Vector3 m_vDesiredPosition;
        private Vector3 m_vDesiredPositionOffset;

        private Vector3 m_vUp;
        private Vector3 m_vVelocity;

        // Physics
        private float m_fStiffness = 1800.0f;
        private float m_fDamping = 600.0f;
        private float m_fMass = 50.0f;

        // Perspective
        private float m_fAspectRatio;
        private float m_fFieldOfView;
        private float m_fNearPlaneDistance;
        private float m_fFarPlaneDistance;
        private Matrix m_ViewMatrix;
        private Matrix m_ProjMatrix;


        //---------------------------------------
        // Class methods
        //---------------------------------------
        public Camera()
        {
            m_Target = null;
            m_vLookAt = Vector3.Zero;
            m_vLookAtOffset = new Vector3(0, 2.8f, 0);
            m_vPosition = Vector3.Zero;
            m_vDesiredPositionOffset = new Vector3(0, 2.0f, 2.0f);
            m_vUp = Vector3.Up;
            m_vVelocity = Vector3.Zero;


            m_fAspectRatio = 4.0f / 3.0f;
            m_fFieldOfView = MathHelper.ToRadians(45.0f);
            m_fNearPlaneDistance = 1.0f;
            m_fFarPlaneDistance = 10000.0f;
        }

        ~Camera() { }

        public void SetTarget(Object3D target)
        {
            m_Target = target;
            UpdateWorldPositions();
            m_vPosition = m_vDesiredPosition;
        }

        public Object3D GetTarget()
        {
            return m_Target;
        }

        public Vector3 GetLookAt()
        {
            UpdateWorldPositions();
            return m_vLookAt;
        }

        public Vector3 GetPosition()
        {
            return m_vPosition;
        }

        public Vector3 GetDesiredPosition()
        {
            UpdateWorldPositions();
            return m_vDesiredPosition;
        }

        public Matrix GetProjectionMatrix()
        {
            return m_ProjMatrix;
        }

        public Matrix GetViewMatrix()
        {
            return m_ViewMatrix;
        }

        private void UpdateWorldPositions()
        {
            // Construct a matrix to transform from object space to worldspace
            Matrix transform = Matrix.Identity;
            transform.Forward = m_Target.GetForward();
            transform.Up = m_vUp;
            transform.Right = Vector3.Cross(m_vUp, m_Target.GetForward());

            // Calculate desired camera properties in world space
            m_vDesiredPosition = m_Target.GetPosition() + Vector3.TransformNormal(m_vDesiredPositionOffset, transform);
            m_vLookAt = m_Target.GetPosition() + Vector3.TransformNormal(m_vLookAtOffset, transform);
        }

        private void UpdateMatrices()
        {
            m_ViewMatrix = Matrix.CreateLookAt(m_vPosition, m_vLookAt, m_vUp);
            m_ProjMatrix = Matrix.CreatePerspectiveFieldOfView(m_fFieldOfView, m_fAspectRatio, m_fNearPlaneDistance, m_fFarPlaneDistance);
        }

        public void Update(GameTime gameTime)
        {
            if (m_Target != null)
            {
                // Target Camera
                UpdateWorldPositions();

                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Calculate spring force
                Vector3 stretch = m_vPosition - m_vDesiredPosition;
                Vector3 force = -m_fStiffness * stretch - m_fDamping * m_vVelocity;

                // Apply acceleration
                Vector3 acceleration = force / m_fMass;
                m_vVelocity += acceleration * elapsed;

                // Apply velocity
                m_vPosition += m_vVelocity * elapsed;

                UpdateMatrices();
            }
            else
            {
                // Free camera (it is moved by the input controls)
            }
        }
    }
}
