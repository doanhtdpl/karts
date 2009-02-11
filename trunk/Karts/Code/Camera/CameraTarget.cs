using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class CameraTarget : Camera
    {
        //------------------------------------------
        // Class members
        //------------------------------------------
        Object3D m_Target;

        private Vector3 m_vLookAtOffset;
        private Vector3 m_vDesiredPosition;
        private Vector3 m_vDesiredPositionOffset;

        private Vector3 m_vVelocity;

        // Physics
        private float m_fStiffness = 1800.0f;
        private float m_fDamping = 600.0f;
        private float m_fMass = 50.0f;


        //------------------------------------------
        // Class methods
        //------------------------------------------
        public CameraTarget() : base()
        {
            m_Target = null;
            m_vLookAtOffset = new Vector3(0, 2.8f, 0);
            m_vDesiredPositionOffset = new Vector3(0, 2000.0f, 3300.0f);
            m_vVelocity = Vector3.Zero;
        }

        public bool Init(int ID, Object3D target)
        {
            base.Init(ID);

            m_Target = target;

            // Set the camera offsets
            m_vDesiredPositionOffset = new Vector3(0.0f, 2000.0f, 3500.0f);
            m_vLookAtOffset = new Vector3(0.0f, 150.0f, 0.0f);

            return true;
        }

        public override ECamType GetType() { return ECamType.ECAMERA_TYPE_TARGET; }

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

        public Vector3 GetDesiredPosition()
        {
            UpdateWorldPositions();
            return m_vDesiredPosition;
        }

        private void UpdateWorldPositions()
        {
            m_vUp = m_Target.GetUp();

            // Construct a matrix to transform from object space to worldspace
            Matrix transform = Matrix.Identity;
            transform.Forward = m_Target.GetForward();
            transform.Up = m_vUp;
            transform.Right = m_Target.GetRight();

            // Calculate desired camera properties in world space
            m_vDesiredPosition = m_Target.GetPosition() + Vector3.TransformNormal(m_vDesiredPositionOffset, transform);
            m_vLookAt = m_Target.GetPosition() + Vector3.TransformNormal(m_vLookAtOffset, transform);
        }


        public override void Update(GameTime gameTime)
        {
            // Target Camera
            UpdateWorldPositions();

            // Calculate spring force
            Vector3 stretch = m_vPosition - m_vDesiredPosition;
            Vector3 force = -m_fStiffness * stretch - m_fDamping * m_vVelocity;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Apply acceleration
            Vector3 acceleration = force / m_fMass;
            m_vVelocity += acceleration * elapsed;

            // Apply velocity
            m_vPosition += m_vVelocity * elapsed;

            UpdateMatrices();
        }
    }
}
