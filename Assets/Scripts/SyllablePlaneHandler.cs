using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter
{
    public class SyllablePlaneHandler : MonoBehaviour
    {
        #region Fields
        private GameObject trackedPlane_m;
        private float trackedDistance_m = 0.0f;
        public SyllableBox BoxHoveredOver;
        [HideInInspector]
        public bool CanDrop = false;
        #endregion

        #region Methods
        public GameObject Pull(SyllableBox box)
        {
            // Pull the syllable plane from the box
            if(box != null)
            {
                GameObject planeGO = box.Box.GetComponentInChildren<SyllablePlane>().gameObject;
                var planeScript = planeGO.GetComponent<SyllablePlane>();
                planeScript.Uncouple();
                box.PullSyllable();

                return planeGO;
            }
            else
            {
                return null;
            }
        }

        public void Drop()
        {
            if (BoxHoveredOver == null
                || CanDrop == false)
            {
                // Drop the plane wherever you are
                trackedPlane_m = null;
            }
            else
            {
                Push(trackedPlane_m, BoxHoveredOver);
                trackedPlane_m = null;
            }
            trackedDistance_m = 0.0f;
        }

        public void StartHold(Vector2 fingerScreenSpace)
        {
            Vector3 worldFingerPos = Camera.main.ScreenToWorldPoint(new Vector3(fingerScreenSpace.x, fingerScreenSpace.y, Camera.main.nearClipPlane));
            StartHold(worldFingerPos);
        }

        public void StartHold(Vector3 currentFingerPos)
        {
            // Raycast to a box and see if it has a syllable associated, if so pull it and make it the tracked plane
            RaycastHit hitInfo;
            Vector3 rayDir = (currentFingerPos - Camera.main.transform.position).normalized;
            Ray r = new Ray(currentFingerPos, rayDir);
            if(Physics.Raycast(r, out hitInfo))
            {
                // if you've collided with an object, determine if it's a box
                SyllableBox box;
                bool boxEmpty;
                SyllablePlane floatingPlane;

                if(DetermineIfBox(hitInfo, out box, out boxEmpty))
                {
                    // Grab the box and uncouple it, then start tracking the plane
                    if (!boxEmpty)
                    {
                        trackedPlane_m = Pull(box);
                        trackedDistance_m = (trackedPlane_m.transform.position - Camera.main.transform.position).magnitude - 0.01f;
                    }
                }
                else if(DetermineIfFloatingPlane(hitInfo, out floatingPlane))
                {
                    trackedPlane_m = floatingPlane.gameObject;
                    trackedDistance_m = (trackedPlane_m.transform.position - Camera.main.transform.position).magnitude - 0.01f;
                }
            }
        }

        public void Hold(Vector2 fingerScreenSpace)
        {
            Vector3 fingerWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(fingerScreenSpace.x, fingerScreenSpace.y, Camera.main.nearClipPlane));
            Hold(fingerWorldPos);
        }

        public void Hold(Vector3 currentFingerPos)
        {
            if (trackedPlane_m != null
                && currentFingerPos != null)
            {
                //float distance = (trackedPlane_m.transform.position - Camera.main.transform.position).magnitude;
                float distance = trackedDistance_m;
                Vector3 dir = (currentFingerPos - Camera.main.transform.position).normalized;
                Vector3 newPos = currentFingerPos + dir * distance;
                trackedPlane_m.transform.position = newPos;
                trackedPlane_m.transform.forward = -Camera.main.transform.forward;

                RaycastHit hitInfo;
                if(GetHoldHitInfo(trackedPlane_m, currentFingerPos, out hitInfo))
                {
                    SyllableBox box;
                    bool boxEmpty;
                    if(DetermineIfBox(hitInfo, out box, out boxEmpty))
                    {
                        // box is valid box
                        BoxHoveredOver = box;
                        CanDrop = boxEmpty;

                        return;
                    }
                }
            }

            // Only hit this logic if you fail to find something you can drop into
            BoxHoveredOver = null;
            CanDrop = false;
        }

        private bool DetermineIfBox(RaycastHit hitInfo, out SyllableBox box, out bool boxEmpty)
        {
            GameObject obj = hitInfo.collider.gameObject;
            var boxPointerScript = obj.GetComponentInParent<SyllableBoxPointer>();
            if(obj != null
                && boxPointerScript != null)
            {
                SyllableBox collidedBox = boxPointerScript.SyllableBox;
                if(collidedBox != null)
                {
                    box = collidedBox;
                    if (collidedBox.Empty)
                    {
                        boxEmpty = true;
                    }
                    else
                    {
                        boxEmpty = false;
                    }

                    return true;
                }
            }

            box = null;
            boxEmpty = false;
            return false;
        }

        private bool DetermineIfFloatingPlane(RaycastHit hitInfo, out SyllablePlane plane)
        {
            GameObject obj = hitInfo.collider.gameObject;
            var planeScript = obj.GetComponentInParent<SyllablePlane>();
            if(obj != null
                && planeScript != null)
            {
                plane = planeScript;
                return true;
            }

            plane = null;
            return false;
        }

        private bool GetHoldHitInfo(GameObject planeGO, Vector3 currentFingerPos, out RaycastHit hitInfo)
        {
            Vector3 planeDir = (planeGO.transform.position - currentFingerPos).normalized;
            Ray r = new Ray(currentFingerPos, planeDir); // Will this cause memory issues? or does it actually dispose like expected?
            return Physics.Raycast(r, out hitInfo);
        }

        public void Push(GameObject planeGO, SyllableBox box)
        {
            if (planeGO != null)
            {
                var planeScript = planeGO.GetComponent<SyllablePlane>();
                if (box != null
                    && box.Syllable == null)
                {
                    planeScript.CoupleTo(box);
                    Syllable syllable = planeScript.Syllable;
                    box.PushSyllable(syllable);

                    // Set the plane's relative position
                    planeGO.transform.localPosition = Vector3.zero;
                    planeGO.transform.localEulerAngles = Vector3.zero;
                }
            }
        }
        #endregion
    }
}