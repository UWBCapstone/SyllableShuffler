using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SyllableShifter.Scripts
{
    /// <summary>
    /// Only use with the Main Unity Camera. The ARCamera provided by Vuforia support has something broken in their back end so that translations from screen or viewport space to world space absolutely do not work.
    /// </summary>
    public class MouseManager : MonoBehaviour
    {
        public SyllablePlaneHandler planeHandler;

        public void Update()
        {
            if (Input.GetMouseButtonDown(LeftClickButton)) // Touch phase == begin
            {
                OnTouch(FingerPos);
            }
            else if (Input.GetMouseButton(LeftClickButton)) // Touch phase == hold
            {
                OnHold(FingerPos);
            }
            else if (Input.GetMouseButtonUp(LeftClickButton)) // Touch phase == end
            {
                OnRelease();
            }
        }

        public void OnTouch(Vector2 fingerPos)
        {
            planeHandler.StartHold(fingerPos);
        }

        public void OnHold(Vector2 fingerPos)
        {
            planeHandler.Hold(fingerPos);
        }

        public void OnRelease()
        {
            planeHandler.Drop();
        }

        #region Helpers
        public Vector3 FingerPos
        {
            get
            {
                return Input.mousePosition;
            }
        }
        public int LeftClickButton
        {
            get
            {
                return 0;
            }
        }
        public int RightClickButton
        {
            get
            {
                return 1;
            }
        }
        #endregion
    }
}