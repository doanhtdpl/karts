using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public int m_IDCamera { get; set; }
        private bool m_bLive;
        public bool Local { get; set; }
        public int LocalPlayerIndex { get; set; }
        public int LocalPlayerIndexCount { get; set; }
        public Viewport Viewport { get; set; }

        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public Player ( UInt32 uID )
        {
            m_uID = PlayerManager.INVALID_PLAYER_ID;
            m_Vehicle = null;
            //m_Driver = null;
            m_sName = null;
            m_vPosition = Vector3.Zero;
            m_vRotation = Vector3.Zero;
            m_IDCamera = CameraManager.INVALID_CAMERA_ID;

            m_uID = uID;
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

        public void Init(string Name, bool local, bool live, int playerIndex, int playerIndexCount)
        {
            m_sName = Name;
            Local = local;
            m_bLive = live;
            LocalPlayerIndex = playerIndex;
            LocalPlayerIndexCount = playerIndexCount;
        }

        public bool Init(Vector3 position, Vector3 rotation, float fScale, string vehicle_name, string driver_name, bool bCamera)
        {
            bool bInitOk = false;

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
                        Object3D target = m_Vehicle;
                        m_IDCamera = CameraManager.GetInstance().CreateCamera(Camera.ECamType.ECAMERA_TYPE_TARGET, true, target, target.GetPosition(), target.GetRotation());
                    }
                }
            }

            if (!bInitOk)
            {
                // Print a message error
            }

            CreateViewport();

            return bInitOk;
        }

        private void CreateViewport()
        {
            int numPlayers = PlayerManager.GetInstance().GetNumLocalPlayers();

            int width = numPlayers > 2 ? 400 : 800;
            int height = numPlayers > 1 ? 300 : 600;

            Viewport v = new Viewport();
            if (numPlayers == 1)
            {
                v.X = 0;
                v.Y = 0;
            }
            else if (numPlayers == 2)
            {
                v.X = 0;
                v.Y = LocalPlayerIndexCount == 0 ? 0 : 300;
            }
            else
            {
                v.X = LocalPlayerIndexCount % 2 == 0 ? 0 : 400;
                v.Y = LocalPlayerIndexCount < 2 ? 0 : 300;
            }
            v.Width = width;
            v.Height = height;
            Viewport = v;

            CameraManager.GetInstance().GetCamera(m_IDCamera).SetAspectRatio((float) ((float)width/(float)height) );
        }

        public void Update(GameTime gameTime)
        {
            InputManager im = InputManager.GetInstance();
            ControllerManager cm = ControllerManager.GetInstance();

            // Switch camera Free/Target
            if (im.isKeyPressed(Keys.Z))
            {
                CameraManager.GetInstance().ActivateCameraFree(!CameraManager.GetInstance().IsActiveCameraFree());
            }

            if (CameraManager.GetInstance().IsActiveCameraFree()) 
                return;

            Vector3 newPos = new Vector3(0, 0, 0);
            float fMove = 00f;

            if (cm.isDown(LocalPlayerIndex, "accelerate"))
            {
                fMove = 100.0f;
            }

            if (cm.isDown(LocalPlayerIndex, "brake"))
            {
                fMove = -100.0f;
            }

            if (cm.isDown(LocalPlayerIndex, "turn_left"))
            {
                m_vRotation.Y += 0.03f;
            }

            if (cm.isDown(LocalPlayerIndex, "turn_right"))
            {
                m_vRotation.Y -= 0.03f;
            }

            m_vPosition = m_vPosition + fMove * GetForward();
            m_Vehicle.SetPosition(m_vPosition);
            m_Vehicle.SetRotationSoft(m_vRotation);
            
            m_Vehicle.Update(gameTime);
            //m_Driver.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            Camera cam = CameraManager.GetInstance().GetActiveCamera();

            m_Vehicle.Draw(gameTime, cam.GetProjectionMatrix(), cam.GetViewMatrix());
            //m_Driver.Draw(gameTime, m_Camera.GetProjectionMatrix(), m_Camera.GetViewMatrix());
        }

        public Vehicle GetVehicle()
        {
            return m_Vehicle;
        }
    }
}
