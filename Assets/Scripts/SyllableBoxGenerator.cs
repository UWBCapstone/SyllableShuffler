using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter.Scripts
{
    public class SyllableBoxGenerator : MonoBehaviour
    {
        public GameObject Prefab;
        private static GameObject Prefab_s;

        public void Update()
        {
            Prefab_s = Prefab;
        }

        public static GameObject Instantiate(string name, Syllable syllable, bool keepInvisible = false)
        {
            var go = GameObject.Instantiate(Prefab_s);
            go.name = name;
            go.SetActive(false);

            var goText = go.GetComponentInChildren<TextMesh>();
            goText.text = syllable.Text;

            // Calculate scales for appropriate text display
            float textScale = goText.text.Length / 5.0f;
            float xScale = (textScale > 0.2f) ? textScale / 0.2f : 1;
            float yScale = (xScale > 3.0f) ? xScale / 3 : 1;
            if(yScale > 1)
            {
                xScale /= yScale;
            }

            // Set scales
            go.transform.localScale = new Vector3(xScale, yScale, go.transform.localScale.z);
            goText.characterSize = textScale;

            // Register information for the syllable plane script
            var plane = go.GetComponentInChildren<SyllablePlane>();
            plane.Register(syllable, plane.gameObject);

            if (!keepInvisible)
            {
                go.SetActive(true);
            }

            return go;
        }
    }
}