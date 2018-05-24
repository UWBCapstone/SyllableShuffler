using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter
{
    public class ARCameraToWorldSpaceTranslator : MonoBehaviour
    {
        public Camera cam;
        public static Camera cam_s;

        public void Update()
        {
            cam_s = cam;
        }

        public static Vector3 ScreenToWorldSpace(Vector2 screenSpace)
        {
            if(cam_s != null)
            {
                int pixelHeight = Screen.height;
                int pixelWidth = Screen.width;
                
                float distance = cam_s.nearClipPlane;
                
                float horizontalAngle = Mathf.Sin(cam_s.fieldOfView / 2.0f * cam_s.aspect);
                float widthHalf = horizontalAngle * distance; // width / 2
                float widthRatio = screenSpace.x / pixelWidth;

                float verticalAngle = Mathf.Sin(cam_s.fieldOfView / 2.0f);
                float heightHalf = verticalAngle * distance;
                float heightRatio = screenSpace.y / pixelHeight;

                float width = widthHalf * 2;
                float height = heightHalf * 2;
                
                Vector3 center = cam_s.transform.position + cam_s.transform.forward * distance;
                Vector3 LL = center - (widthHalf * cam_s.transform.right) - (heightHalf * cam_s.transform.up);
                Vector3 point = LL + (widthRatio * width * cam_s.transform.right) + (heightRatio * height * cam_s.transform.up);

                return point;
            }

            return Vector3.zero;
        }

        public static Vector3 ViewportToWorldSpace(Vector2 viewportSpace)
        {
            if(cam_s != null)
            {

                float distance = cam_s.nearClipPlane;

                float horizontalAngle = Mathf.Sin(cam_s.fieldOfView / 2.0f * cam_s.aspect);
                float widthHalf = horizontalAngle * distance; // width / 2
                float widthRatio = viewportSpace.x;

                float verticalAngle = Mathf.Sin(cam_s.fieldOfView / 2.0f);
                float heightHalf = verticalAngle * distance;
                float heightRatio = viewportSpace.y;

                float width = widthHalf * 2;
                float height = heightHalf * 2;

                Vector3 center = cam_s.transform.position + cam_s.transform.forward * distance;
                Vector3 LL = center - (widthHalf * cam_s.transform.right) - (heightHalf * cam_s.transform.up);
                Vector3 point = LL + (widthRatio * width * cam_s.transform.right) + (heightRatio * height * cam_s.transform.up);

                return point;
            }

            return Vector3.zero;
        }

        #region Properties
        public Camera Camera
        {
            get
            {
                return cam;
            }
        }
        #endregion
    }
}