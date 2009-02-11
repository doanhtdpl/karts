using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using System.Diagnostics;

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
        int m_iOldActiveCameraID;
        int m_iIDCameraCounter;

        //-------------------------------------------------
        // Class methods
        //-------------------------------------------------
        public CameraManager(Game game) : base(game)
        {
            m_iActiveCameraID = INVALID_CAMERA_ID;
            m_iIDCameraCounter = INVALID_CAMERA_ID;
            m_iOldActiveCameraID = INVALID_CAMERA_ID;
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
            m_CameraManager.CreateCamera(Camera.ECamType.ECAMERA_TYPE_FREE, true, null, Vector3.Zero, Vector3.Zero);

            return m_CameraManager;
        }

        public void ActivateCameraFree(bool activate)
        {
            Camera cam = GetActiveCamera();

            if (activate)
            {
                if (cam.GetCameraType() != Camera.ECamType.ECAMERA_TYPE_FREE)
                {
                    m_iOldActiveCameraID = cam.GetID();
                    Vector3 pos = cam.GetPosition();
                    Vector3 rot = cam.GetRotation();
                    cam = GetCamera(Camera.ECamType.ECAMERA_TYPE_FREE);

                    if (cam == null)
                    {
                        CreateCamera(Camera.ECamType.ECAMERA_TYPE_FREE, true, null, pos, rot);
                    }
                    else
                    {
                        cam.SetPosition(pos);
                        cam.SetRotation(rot);
                        m_iActiveCameraID = cam.GetID();
                    }
                }
            }
            else
            {
                if (m_iOldActiveCameraID != INVALID_CAMERA_ID)
                {
                    m_iActiveCameraID = m_iOldActiveCameraID;
                }
            }
        }

        public int CreateCamera(Camera.ECamType type, bool bActive, Object3D target, Vector3 pos, Vector3 rot)
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
                newCamera.Init(iCameraID, pos, rot);
                m_CameraList.Add(newCamera);
            }

            if (bActive)
            {
                // we set as active camera by default the first created one
                m_iActiveCameraID = iCameraID;
            }

            return iCameraID;
        }

        public bool IsActiveCameraFree()
        {
            Camera cam = GetActiveCamera();

            return cam == null ? false : cam.GetCameraType() == Camera.ECamType.ECAMERA_TYPE_FREE;
        }

        public bool IsActiveCameraTarget()
        {
            Camera cam = GetActiveCamera();

            return cam == null ? false : cam.GetCameraType() == Camera.ECamType.ECAMERA_TYPE_TARGET;
        }

        public Camera GetActiveCamera()
        {
            return m_CameraList.Find(new FindCameraID(m_iActiveCameraID).CompareID);
        }

        public Camera.ECamType GetActiveCameraType()
        {
            Camera cam = m_CameraList.Find(new FindCameraID(m_iActiveCameraID).CompareID);

            return cam == null ? Camera.ECamType.ECAMERA_TYPE_INVALID : cam.GetCameraType();
        }

        public int GetActiveCameraID()
        {
            Camera cam = m_CameraList.Find(new FindCameraID(m_iActiveCameraID).CompareID);

            return cam == null ? INVALID_CAMERA_ID : cam.GetID();
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

        public Camera GetCamera(Camera.ECamType type)
        {
            return m_CameraList.Find(new FindCameraType(type).CompareTypes);
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

        private struct FindCameraType
        {
            Camera.ECamType eType;

            public FindCameraType(Camera.ECamType _type) { eType = _type; }

            public bool CompareTypes(Camera c)
            {
                return c.GetCameraType() == eType;
            }
        }
    }
}
