using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter
{
    public class SyllablePlaneHandler : MonoBehaviour
    {
        #region Fields
        private GameObject trackedPlane_m;
        public SyllableBox BoxHoveredOver;
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
            }
        }

        public void StartHold(Vector2 fingerScreenSpace)
        {
            Vector3 worldFingerPos = Camera.main.ViewportToScreenPoint(new Vector3(fingerScreenSpace.x, fingerScreenSpace.y, Camera.main.nearClipPlane));
            StartHold(worldFingerPos);
        }

        public void StartHold(Vector3 currentFingerPos)
        {
            // Raycast to a box and see if it has a syllable associated, if so pull it and make it the tracked plane
            RaycastHit hitInfo;
            Ray r = new Ray(currentFingerPos, Camera.main.transform.forward);
            if(Physics.Raycast(r, out hitInfo))
            {
                // if you've collided with an object, determine if it's a box
                SyllableBox box;
                bool boxEmpty;
                if(DetermineIfBox(hitInfo, out box, out boxEmpty))
                {
                    // Grab the box and uncouple it, then start tracking the plane
                    if (!boxEmpty)
                    {
                        trackedPlane_m = Pull(box);
                    }
                }
            }
        }

        public void Hold(Vector2 fingerScreenSpace)
        {
            Vector3 fingerWorldPos = Camera.main.ViewportToWorldPoint(new Vector3(fingerScreenSpace.x, fingerScreenSpace.y, Camera.main.nearClipPlane));
            Hold(fingerWorldPos);
        }

        public void Hold(Vector3 currentFingerPos)
        {
            if (trackedPlane_m != null
                && currentFingerPos != null)
            {
                float distance = (trackedPlane_m.transform.position - Camera.main.transform.position).magnitude;
                Vector3 newPos = currentFingerPos + Camera.main.transform.forward * distance;
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
            var boxPointerScript = obj.GetComponent<SyllableBoxPointer>();
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

        private bool GetHoldHitInfo(GameObject planeGO, Vector3 currentFingerPos, out RaycastHit hitInfo)
        {
            Vector3 planeDir = (planeGO.transform.position - currentFingerPos).normalized;
            Ray r = new Ray(planeGO.transform.position, planeDir); // Will this cause memory issues? or does it actually dispose like expected?
            return Physics.Raycast(r, out hitInfo);
        }

        public void Push(GameObject planeGO, SyllableBox box)
        {
            if (planeGO != null)
            {
                var planeScript = planeGO.GetComponent<SyllablePlane>();
                if (box != null
                    && box.Syllable != null)
                {
                    planeScript.CoupleTo(box);
                }
            }
        }
        #endregion
    }
}