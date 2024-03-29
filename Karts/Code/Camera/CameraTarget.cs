﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
        private float m_fStiffness = 3000.0f;
        private float m_fDamping = 600.0f;
        private float m_fMass = 10.0f;


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
            base.Init(ID, ECamType.ECAMERA_TYPE_TARGET, target.GetPosition(), target.GetRotation());

            m_Target = target;

            // Set the camera offsets
            m_vDesiredPositionOffset = new Vector3(0.0f, 1500.0f, 3500.0f);
            m_vLookAtOffset = new Vector3(0.0f, 350.0f, 0.0f);

            return true;
        }

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
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (m_eType == ECamType.ECAMERA_TYPE_TARGET)
            {
                // Target Camera
                UpdateWorldPositions();

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
            else if (m_eType == ECamType.ECAMERA_TYPE_FREE)
            {
                // Free camera (it is moved by the input controls)
                bool bMoveUp = InputManager.GetInstance().isKeyDown(Keys.W);
                bool bMoveDown = InputManager.GetInstance().isKeyDown(Keys.S);
                bool bMoveLeft = InputManager.GetInstance().isKeyDown(Keys.A);
                bool bMoveRight = InputManager.GetInstance().isKeyDown(Keys.D);

                float fValueZ = 0.0f;
                float fValueX = 0.0f;

                if (bMoveUp)
                {
                    fValueZ = fValueZ + 5000.0f * elapsed;
                }

                if (bMoveDown)
                {
                    fValueZ = fValueZ - 5000.0f * elapsed;
                }

                if (bMoveLeft)
                {
                    fValueX = fValueX - 5000.0f * elapsed;
                }

                if (bMoveRight)
                {
                    fValueX = fValueX + 5000.0f * elapsed;
                }

                bool bTurnLeft = InputManager.GetInstance().isKeyDown(Keys.Left);
                bool bTurnRight = InputManager.GetInstance().isKeyDown(Keys.Right);
                bool bTurnUp = InputManager.GetInstance().isKeyDown(Keys.Up);
                bool bTurnDown = InputManager.GetInstance().isKeyDown(Keys.Down);

                if (bTurnRight)
                {
                    m_vRotation.Y -= 0.8f * elapsed;
                }

                if (bTurnLeft)
                {
                    m_vRotation.Y += 0.8f * elapsed;
                }

                if (bTurnDown)
                {
                    m_vRotation.X -= 0.8f * elapsed;
                }

                if (bTurnUp)
                {
                    m_vRotation.X += 0.8f * elapsed;
                }

                // We first calculate the rotation and then translate
                Vector3 fwd = GetForward();
                m_vPosition = m_vPosition + fValueZ * fwd + fValueX * GetRight();
                m_vLookAt = m_vPosition + fwd;

                // We keep the up vector with the default value
                //m_vUp = GetUp();

                m_ViewMatrix = Matrix.CreateLookAt(m_vPosition, m_vLookAt, m_vUp);
            }
        }
    }
}
