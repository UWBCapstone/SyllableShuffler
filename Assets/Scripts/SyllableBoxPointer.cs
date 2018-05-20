using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter
{
    public class SyllableBoxPointer : MonoBehaviour
    {
        private SyllableBox box_m;

        public void SetBox(SyllableBox box)
        {
            box_m = box;
        }

        public SyllableBox SyllableBox
        {
            get
            {
                return box_m;
            }
        }
    }
}