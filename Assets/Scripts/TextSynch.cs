using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter
{
    public class TextSynch : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            var syllableBox = GetAssociatedBox();
            if(syllableBox != null)
            {
                var textMesh = gameObject.GetComponent<TextMesh>();
                if(textMesh != null)
                {
                    textMesh.text = syllableBox.Syllable;
                    // Doesn't handle the synchronization of the scaling of the box with the text - should it?
                    GetAssociatedSyllablePlane().SetSyllable(new Syllable(textMesh.text)); // Synch the registered syllable text for the box
                }
                else
                {
                    Debug.LogError("TextSynch: TextMesh not found for synchronization purposes!");
                }
            }
        }

        public SyllableBox GetAssociatedBox()
        {
            //GameObject boxObj = gameObject.transform.root.gameObject;
            if (gameObject.transform.parent != null)
            {
                if (gameObject.transform.parent.parent != null)
                {
                    GameObject boxObj = gameObject.transform.parent.parent.gameObject;
                    var boxPointer = boxObj.GetComponent<SyllableBoxPointer>();
                    if (boxPointer != null)
                    {
                        return boxPointer.SyllableBox;
                    }
                }
            }

            return null;
        }

        public SyllablePlane GetAssociatedSyllablePlane()
        {
            if(gameObject.transform.parent != null)
            {
                GameObject planeObj = gameObject.transform.parent.gameObject;
                var planeScript = planeObj.GetComponent<SyllablePlane>();
                if(planeScript != null)
                {
                    return planeScript;
                }
            }

            return null;
        }
    }
}