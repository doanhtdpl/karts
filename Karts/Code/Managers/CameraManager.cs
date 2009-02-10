using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    

    class CameraManager : GameComponent
    {
        // Constants
        public const int INVALID_CAMERA_ID = -1;

        //-------------------------------------------------
        // Class members
        //-------------------------------------------------
        private List<Camera> m_CameraList = new List<Camera>();
        public static CameraManager m_CameraManager = null;
        int m_iActiveCameraID;
        int m_iIDCameraCounter;

        //-------------------------------------------------
        // Class methods
        //-------------------------------------------------
        public CameraManager(Game game) : base(game)
        {
            m_iActiveCameraID = INVALID_CAMERA_ID;
            m_iIDCameraCounter = INVALID_CAMERA_ID;
        }

        ~CameraManager(){ }

        public static CameraManager GetInstance()
        {
            return m_CameraManager;
        }

        public static CameraManager Init(Game game)
        {
            if (m_CameraManager == null)
                m_CameraManager = new CameraManager(game);

            // By default we create a free camera
            //m_CameraManager.CreateCamera(Camera.ECamType.ECAMERA_TYPE_FREE, null);

            return m_CameraManager;
        }

        public int CreateCamera(Camera.ECamType type, Object3D target)
        {
            int iCameraID = ++m_iIDCameraCounter;

            if (GetCamera(iCameraID) != null)
                return INVALID_CAMERA_ID;

            if (type == Camera.ECamType.ECAMERA_TYPE_TARGET)
            {
                CameraTarget newCamera = new CameraTarget();
                newCamera.Init(iCameraID, target);
                m_CameraList.Add(newCamera);
            }
            else if (type == Camera.ECamType.ECAMERA_TYPE_FREE)
            {
                CameraFree newCamera = new CameraFree();
                newCamera.Init(iCameraID);
                m_CameraList.Add(newCamera);
            }

            if (m_iActiveCameraID < 0)
            {
                // we set as active camera by default the first created one
                m_iActiveCameraID = iCameraID;
            }

            return iCameraID;
        }

        public Camera GetActiveCamera()
        {
            return m_CameraList.Find(new FindCameraID(m_iActiveCameraID).CompareID);
        }

        public void SetActiveCamera(int id)
        {
            if (id < m_CameraList.Count)
            {
                m_iActiveCameraID = id;
            }
        }

        public Camera GetCamera(int id)
        {
            return m_CameraList.Find(new FindCameraID(id).CompareID);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Camera c in m_CameraList)
	        {
    		    c.Update(gameTime);
	        }
        }

        //-------------------------------------------------
        // Class predicates
        //-------------------------------------------------
        private struct FindCameraID
        {
            int iID;

            public FindCameraID(int _id) { iID = _id; }
            public bool CompareID(Camera c)
            {
                return c.GetID() == iID;
            }
        }
    }
}
