using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace SyllableShifter
{
    public static class VuforiaPool
    {
        #region Fields
        private static Dictionary<GameObject, int> pool_m;
        private static Dictionary<int, GameObject> available_m;
        private static Dictionary<int, GameObject> unavailable_m;
        public static readonly string VuforiaNameKey = "VuforiaTarget";
        #endregion

        #region Methods
        static VuforiaPool()
        {
            pool_m = new Dictionary<GameObject, int>();
            available_m = new Dictionary<int, GameObject>();
            unavailable_m = new Dictionary<int, GameObject>();

            GatherVuforiaObjectsFromScene();
        }

        public static GameObject Pull(int index)
        {
            if (available_m.ContainsKey(index))
            {
                GameObject go = available_m[index];
                available_m.Remove(index);
                unavailable_m.Add(index, go);

                var toggle = go.GetComponent<VuforiaTakenToggle>();
                toggle.Taken = true;

                return go;
            }

            return null;
        }

        public static GameObject PullNext(out int index)
        {
            if(available_m.Count > 0)
            {
                List<KeyValuePair<int, GameObject>> temp = available_m.ToList<KeyValuePair<int, GameObject>>();
                var temp2 = temp.OrderBy(x => x.Key);
                temp = new List<KeyValuePair<int, GameObject>>(temp2);

                if(temp.Count > 0)
                {
                    // Return the lowest index value GameObject from the list
                    index = temp[0].Key;
                    return Pull(index);
                }
            }

            index = -1;
            return null;
        }

        public static bool Release(GameObject go)
        {
            if (pool_m.ContainsKey(go))
            {
                int index = pool_m[go];
                if (unavailable_m.ContainsKey(index))
                {
                    available_m.Add(index, go);
                    unavailable_m.Remove(index);

                    var toggle = go.GetComponent<VuforiaTakenToggle>();
                    toggle.Taken = false;

                    return true;
                }
            }

            return false;
        }

        public static void GatherVuforiaObjectsFromScene()
        {
            // Search for and grab all GameObjects that have the 
            var allGOs = GameObject.FindObjectsOfType<GameObject>();
            foreach(var go in allGOs)
            {
                if (go.name.Contains(VuforiaNameKey))
                {
                    // Gather the index of the Vuforia object from its name
                    string[] nameComponents = go.name.Split(new char[1] { '_' });
                    int index = int.Parse(nameComponents[nameComponents.Length - 1]);

                    pool_m.Add(go, index);
                    available_m.Add(index, go);

                    var toggleScript = go.GetComponent<VuforiaTakenToggle>();
                    if(toggleScript == null)
                    {
                        toggleScript = go.AddComponent<VuforiaTakenToggle>();
                    }
                    toggleScript.Taken = false;

                    Debug.Log("VuforiaPool: Adding " + go.name + " to Vuforia pool of possible targets. Assigning index " + index);
                }
                else
                {
                    Debug.Log("VuforiaPool: Ignoring GameObject " + go.name);
                }
            }
        }
        #endregion

        #region Properties
        public static int Count
        {
            get
            {
                return pool_m.Count;
            }
        }
        public static int NumAvailable
        {
            get
            {
                return available_m.Count;
            }
        }
        public static int NumUnavailable
        {
            get
            {
                return unavailable_m.Count;
            }
        }
        #endregion
    }
}