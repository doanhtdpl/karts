using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System.Diagnostics;


namespace Karts.Code
{
    class CameraFree : Camera
    {
        //------------------------------------------
        // Class methods
        //------------------------------------------
        public override ECamType GetCameraType() { return ECamType.ECAMERA_TYPE_FREE; }

        public new bool Init(int ID, Vector3 pos, Vector3 rot)
        {
            base.Init(ID, pos, rot);

            // Init the view and projection matrix
            UpdateMatrices();

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

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
