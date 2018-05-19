using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter
{
    public class SyllableBox
    {
        #region Fields
        private Syllable syllable_m;
        private GameObject boxObj_m;

        //private static Mesh cachedMesh_m;
        #endregion

        #region Static Methods
        public static string GetBoxName(string syllable = "")
        {
            return syllable.Substring(0, 1).ToUpper() + syllable.Substring(1) + "Box";
        }

        //public static void CacheMesh()
        //{
        //    GameObject prim = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    Mesh m = prim.GetComponent<MeshFilter>().sharedMesh;
        //    cachedMesh_m = new Mesh();
        //    cachedMesh_m.SetVertices(new List<Vector3>(m.vertices));
        //    cachedMesh_m.SetTriangles(m.triangles, 0);
        //    cachedMesh_m.SetUVs(0, new List<Vector2>(m.uv));
        //    cachedMesh_m.RecalculateNormals();
        //    cachedMesh_m.RecalculateBounds();
        //    cachedMesh_m.name = prim.name;
        //}
        #endregion

        #region Methods
        public SyllableBox()
        {
            syllable_m = new Syllable();
            boxObj_m = initBox(syllable_m);
        }

        public SyllableBox(Syllable syllable)
        {
            boxObj_m = initBox(syllable_m);
        }

        private GameObject initBox(string syllable = "")
        {
            bool keepInvisible = true;
            string name = GetBoxName(syllable);
            var boxGO = SyllableShifter.Scripts.SyllableBoxGenerator.Instantiate(name, new Syllable(syllable), keepInvisible);
            return boxGO;
        }

        #region Positioning
        public bool IsLeftOf(SyllableBox other)
        {
            Vector3 otherRight = other.transform.right;
            Vector3 pos = transform.position;
            Vector3 vec = pos - other.transform.position;
            return Vector3.Dot(otherRight, vec) > 0;
        }

        public bool IsRightOf(SyllableBox other)
        {
            Vector3 otherRight = other.transform.right;
            Vector3 pos = transform.position;
            Vector3 vec = pos - other.transform.position;
            return Vector3.Dot(otherRight, vec) < 0;
        }
        #endregion
        #endregion


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
        public GameObject Box
        {
            get
            {
                return boxObj_m;
            }
        }
        public Transform transform
        {
            get
            {
                if (boxObj_m != null)
                {
                    return boxObj_m.transform;
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