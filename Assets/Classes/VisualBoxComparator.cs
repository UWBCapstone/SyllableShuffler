using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter
{
    /// <summary>
    /// Simple comparator that checks for viewport x value comparison, then viewport y value comparison, then viewport z value comparison.
    /// </summary>
    public class VisualBoxComparator : Comparer<SyllableBox>
    {
        //int IComparer<SyllableBox>.Compare(object a, object b)
        //{
        //    SyllableBox b1 = (SyllableBox)a;
        //    SyllableBox b2 = (SyllableBox)b;

        //    Vector3 pos1 = Camera.main.WorldToViewportPoint(b1.gameObject.transform.position);
        //    Vector3 pos2 = Camera.main.WorldToViewportPoint(b2.gameObject.transform.position);

        //    if(pos1.x == pos2.x)
        //    {
        //        if(pos1.y == pos2.y)
        //        {
        //            if(pos1.z == pos2.z)
        //            {
        //                return 0;
        //            }
        //            else
        //            {
        //                return pos1.z.CompareTo(pos2.z);
        //            }
        //        }
        //        else
        //        {
        //            return pos1.y.CompareTo(pos2.y);
        //        }
        //    }
        //    else
        //    {
        //        return pos1.x.CompareTo(pos2.x);
        //    }
        //}

        public override int Compare(SyllableBox b1, SyllableBox b2)
        {
            Vector3 pos1 = Camera.main.WorldToViewportPoint(b1.gameObject.transform.position);
            Vector3 pos2 = Camera.main.WorldToViewportPoint(b2.gameObject.transform.position);

            if (pos1.x == pos2.x)
            {
                if (pos1.y == pos2.y)
                {
                    if (pos1.z == pos2.z)
                    {
                        return 0;
                    }
                    else
                    {
                        return pos1.z.CompareTo(pos2.z);
                    }
                }
                else
                {
                    return pos1.y.CompareTo(pos2.y);
                }
            }
            else
            {
                return pos1.x.CompareTo(pos2.x);
            }
        }
    }
}