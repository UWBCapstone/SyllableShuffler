using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter
{
    public class TouchscreenManager : MonoBehaviour
    {
        public SyllablePlaneHandler planeHandler;

        public void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Began)
                {
                    OnTouch(touch.position);
                }
                else if(touch.phase == TouchPhase.Moved
                    || touch.phase == TouchPhase.Stationary)
                {
                    OnHold(touch.position);
                }
                else if(touch.phase == TouchPhase.Ended)
                {
                    OnRelease();
                }
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
    }
}