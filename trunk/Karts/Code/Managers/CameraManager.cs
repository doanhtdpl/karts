using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class CameraManager : GameComponent
    {
        //-------------------------------------------------
        // Class members
        //-------------------------------------------------
        private List<Camera> m_CameraList = new List<Camera>();
        public static CameraManager m_CameraManager = null;
        int iActiveCameraID;

        //-------------------------------------------------
        // Class methods
        //-------------------------------------------------
        public CameraManager(Game game) : base(game)
        {
            iActiveCameraID = -1;
        }

        ~CameraManager() { }

        public static CameraManager GetInstance()
        {
            return m_CameraManager;
        }

        public static CameraManager Init(Game game)
        {
            if (m_CameraManager == null)
                m_CameraManager = new CameraManager(game);

            return m_CameraManager;
        }

        public int CreateCamera(Object3D target)
        {
            int iCameraID = m_CameraList.Count;
            Camera newCam = new Camera();

            newCam.SetTarget(target);
            m_CameraList.Add(newCam);

            if (iActiveCameraID < 0)
            {
                // we set as active camera by default the first created one
                iActiveCameraID = iCameraID;
            }

            return iCameraID;
        }

        public Camera GetActiveCamera()
        {
            if (iActiveCameraID < 0 || iActiveCameraID >= m_CameraList.Count)
                return null;

            return m_CameraList[iActiveCameraID];
        }

        public void SetActiveCamera(int id)
        {
            if (id < m_CameraList.Count)
            {
                iActiveCameraID = id;
            }
        }

        public Camera GetCamera(int id)
        {
            if (id < m_CameraList.Count)
            {
                return m_CameraList[id];
            }

                return null;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Camera c in m_CameraList)
	        {
    		    c.Update(gameTime);
	        }
        }
    }
}
