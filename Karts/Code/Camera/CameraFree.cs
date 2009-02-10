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
        public new ECamType GetType() { return ECamType.ECAMERA_TYPE_FREE; }

        public new bool Init(int ID)
        {
            base.Init(ID);

            // Init the view and projection matrix
            UpdateMatrices();

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            // Free camera (it is moved by the input controls)
            bool bMoveUp = InputManager.GetInstance().isKeyDown(Keys.W);
            bool bMoveDown = InputManager.GetInstance().isKeyDown(Keys.S);
            bool bMoveLeft = InputManager.GetInstance().isKeyDown(Keys.A);
            bool bMoveRight = InputManager.GetInstance().isKeyDown(Keys.D);

            float fValue = 0.0f;
            float fRightV = 0.0f;

            if (bMoveUp)
            {
                fValue = 50.0f;
            }

            if (bMoveDown)
            {
                fValue = -50.0f;
            }

            if (bMoveLeft)
            {
                fRightV = -50.0f;
            }

            if (bMoveRight)
            {
                fRightV = 50.0f;
            }

            bool bTurnLeft = InputManager.GetInstance().isKeyDown(Keys.Left);
            bool bTurnRight = InputManager.GetInstance().isKeyDown(Keys.Right);
            bool bTurnUp = InputManager.GetInstance().isKeyDown(Keys.Up);
            bool bTurnDown = InputManager.GetInstance().isKeyDown(Keys.Down);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector3 fwd = m_ViewMatrix.Forward;

            if (bTurnUp)
            {
                fwd = Vector3.TransformNormal(fwd, Matrix.CreateFromAxisAngle(m_ViewMatrix.Right, 0.5f * elapsed));
            }

            if (bTurnDown)
            {
                fwd = Vector3.TransformNormal(fwd, Matrix.CreateFromAxisAngle(m_ViewMatrix.Right, -0.5f * elapsed));
            }

            if (bTurnLeft)
            {
                fwd = Vector3.TransformNormal(fwd, Matrix.CreateFromAxisAngle(m_ViewMatrix.Up, 0.5f * elapsed));
            }

            if (bTurnRight)
            {
                fwd = Vector3.TransformNormal(fwd, Matrix.CreateFromAxisAngle(m_ViewMatrix.Up, -0.5f * elapsed));
            }

            //m_vPosition = m_vPosition + m_ViewMatrix.Forward * fValue + m_ViewMatrix.Right * fRightV;

            m_vLookAt = m_vPosition + fwd; // LookAt is a position, not a direction

            //m_vUp = m_ViewMatrix.Up;

            Debug.Print("look_at: " + m_vLookAt);
            Debug.Print("fwd: " + fwd);
            Debug.Print("position: " + m_vPosition.ToString() + " lookat: " + m_vLookAt.ToString());

            m_ViewMatrix = Matrix.CreateLookAt(m_vPosition, m_vLookAt, m_vUp);
            Debug.Print("fwd after: " + m_ViewMatrix.Forward);
            Debug.Print("");
        }
    }
}
