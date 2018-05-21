using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter
{
    public class SyllablePlane : MonoBehaviour
    {
        #region Fields
        private Syllable syllable_m;
        private GameObject plane_m;
        ////private TextMesh textMesh_m;
        //private SyllableBox associatedBox_m;
        #endregion
        

        //public SyllablePlane(Syllable syllable, GameObject plane, SyllableBox associatedBox)
        //{
        //    Register(syllable, plane, associatedBox);
        //}

        public void Register(Syllable syllable, GameObject plane)
        {
            syllable_m = syllable;
            plane_m = plane;
            //associatedBox_m = associatedBox;
        }

        public void SetSyllable(Syllable syllable)
        {
            syllable_m = syllable;
        }

        public GameObject Uncouple()
        {
            if(plane_m != null)
            {
                plane_m.transform.parent = null;
            }

            return plane_m;
        }

        public GameObject CoupleTo(SyllableBox box)
        {
            // Set the transform to be correct by applying it as a child of the box
            if(plane_m != null
                && box != null)
            {
                plane_m.transform.parent = box.transform;
            }

            return box.Box;
        }

        #region Properties
        public Syllable Syllable
        {
            get
            {
                return syllable_m;
            }
            set
            {
                syllable_m = value;
            }
        }
        public GameObject Plane
        {
            get
            {
                return plane_m;
            }
        }
        //public SyllableBox Box
        //{
        //    get
        //    {
        //        return associatedBox_m;
        //    }
        //}
        public Transform transform
        {
            get
            {
                if(plane_m != null)
                {
                    return plane_m.transform;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
    }
}