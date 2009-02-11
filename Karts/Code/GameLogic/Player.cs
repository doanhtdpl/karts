using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System.Diagnostics;

namespace Karts.Code
{
    class Player : Object3D
    {
        // ------------------------------------------------
        // Class members
        // ------------------------------------------------
        private Vehicle m_Vehicle;
        //private Driver m_Driver;
        private UInt32 m_uID;
        private string m_sName;
        private int m_IDCamera;


        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public Player ()
        {
            m_uID = PlayerManager.INVALID_PLAYER_ID;
            m_Vehicle = null;
            //m_Driver = null;
            m_sName = null;
            m_vPosition = Vector3.Zero;
            m_vRotation = Vector3.Zero;
            m_IDCamera = CameraManager.INVALID_CAMERA_ID;
        }

        ~Player ()
        {
            m_uID = PlayerManager.INVALID_PLAYER_ID;
            m_Vehicle = null;
            //m_Driver = null;
            m_sName = null;
            m_vPosition = Vector3.Zero;
            m_vRotation = Vector3.Zero;
            m_IDCamera = CameraManager.INVALID_CAMERA_ID;
        }

        public UInt32 GetID() { return m_uID; }
        public string GetName() { return m_sName; }

        public bool Init(Vector3 position, Vector3 rotation, float fScale, string Name, UInt32 uID, string vehicle_name, string driver_name, bool bCamera)
        {
            bool bInitOk = false;

            m_uID = uID;
            m_sName = Name;
            m_vPosition = position;
            m_vRotation = rotation;

            // Load and init the vehicle model
            m_Vehicle = new Vehicle();
            bInitOk = m_Vehicle.Init(position, rotation, fScale, vehicle_name);

            if (bInitOk)
            {
                /*
                // Load and init the driver model
                m_Driver = new Driver();
                bInitOk = m_Driver.Init(driver_name);
                */
                if (bInitOk)
                {
                    if (bCamera)
                    {
                        Object3D target = m_Vehicle.GetObject3D();
                        m_IDCamera = CameraManager.GetInstance().CreateCamera(Camera.ECamType.ECAMERA_TYPE_TARGET, true, target, target.GetPosition(), target.GetRotation());
                    }
                }
            }

            if (!bInitOk)
            {
                // Print a message error
            }

            return bInitOk;
        }

        public void Update(GameTime gameTime)
        {
            InputManager im = InputManager.GetInstance();

            // Switch camera Free/Target
            if (im.isKeyPressed(Keys.Z))
            {
                CameraManager.GetInstance().ActivateCameraFree(!CameraManager.GetInstance().IsActiveCameraFree());
            }

            if (CameraManager.GetInstance().IsActiveCameraFree()) 
                return;

            Vector3 newPos = new Vector3(0, 0, 0);
            float fMove = 00f;

            if (im.isKeyDown(Keys.Up))
            {
                fMove = 100.0f;
            }

            if (im.isKeyDown(Keys.Down))
            {
                fMove = -100.0f;
            }

            if (im.isKeyDown(Keys.Left))
            {
                m_vRotation.Y += 0.2f;
            }

            if (im.isKeyDown(Keys.Right))
            {
                m_vRotation.Y -= 0.2f;
            }

            m_vPosition = m_vPosition + fMove * GetForward();
            m_Vehicle.GetObject3D().SetPosition(m_vPosition);
            m_Vehicle.SetRotation(m_vRotation);
            
            m_Vehicle.Update(gameTime);
            //m_Driver.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            Camera cam = CameraManager.GetInstance().GetActiveCamera();

            m_Vehicle.Draw(gameTime, cam.GetProjectionMatrix(), cam.GetViewMatrix());
            //m_Driver.Draw(gameTime, m_Camera.GetProjectionMatrix(), m_Camera.GetViewMatrix());
        }
    }
}
