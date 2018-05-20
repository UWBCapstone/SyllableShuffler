using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq; // For shuffling syllables

namespace SyllableShifter
{
    public class SyllableBoxSet
    {
        #region Fields
        private Word word_m;
        private Syllable[] shuffledSyllables_m;
        public List<SyllableBox> Boxes;
        #endregion

        #region Methods
        #region Init
        public SyllableBoxSet(Word word)
        {
            bool shuffle = true;
            SetWord(word, shuffle);
            Boxes = GenerateBoxes(word);
        }
        #endregion
        
        public Syllable[] ShuffleSyllables(Syllable[] syllablesToShuffle)
        {
            if(shuffledSyllables_m != null)
            {
                List<Syllable> temp = new List<Syllable>(shuffledSyllables_m);
                var rnd = new System.Random();
                temp.OrderBy(item => rnd.Next());
                return temp.ToArray();
            }
            else
            {
                return null;
            }
        }

        public bool IsCorrectOrder(Syllable[] syllables)
        {
            for(int i = 0; i < syllables.Length; i++)
            {
                if(!IsSyllableInCorrectSpot(syllables[i], i))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsSyllableInCorrectSpot(Syllable syllable, int expectedSyllableIndex)
        {
            if(expectedSyllableIndex > word_m.SyllableCount)
            {
                return false;
            }
            else
            {
                return syllable.Matches(word_m[expectedSyllableIndex]);
            }
        }

        public void SetWord(Word word, bool shuffle = true)
        {
            if (word != null)
            {
                word_m = word;
                shuffledSyllables_m = new Syllable[word_m.SyllableCount];
                for (int i = 0; i < word_m.SyllableCount; i++)
                {
                    shuffledSyllables_m[i] = word_m[i];
                }

                if (shuffle)
                {
                    shuffledSyllables_m = ShuffleSyllables(shuffledSyllables_m);
                    while (IsCorrectOrder(shuffledSyllables_m))
                    {
                        shuffledSyllables_m = ShuffleSyllables(shuffledSyllables_m);
                    }
                }
            }
        }

        public List<SyllableBox> GenerateBoxes(Word word)
        {
            List<SyllableBox> boxes = new List<SyllableBox>();

            // Grab the Vuforia objects we're going to be associating with the boxes
            List<KeyValuePair<int, GameObject>> vuforiaParentPairs = GrabVuforiaObjects(word.SyllableCount);
            //List<GameObject> vuforiaParents = GrabVuforiaObjects(word.SyllableCount);

            // Generate the box script, which generates the box object and attaches the necessary scripts
            for(int i = 0; i < word.SyllableCount; i++)
            {
                SyllableBox box = new SyllableBox(word[i], vuforiaParentPairs[i].Value);
                boxes.Add(box);
                Debug.Log("SyllableBoxSet: Generating box for Vuforia ID pattern " + vuforiaParentPairs[i].Key);
            }

            return boxes;
        }

        public List<KeyValuePair<int, GameObject>> GrabVuforiaObjects(int count)
        {
            if (count <= VuforiaPool.NumAvailable)
            {
                List<KeyValuePair<int, GameObject>> objs = new List<KeyValuePair<int, GameObject>>();

                for (int i = 0; i < count; i++)
                {
                    int index;
                    GameObject go = VuforiaPool.PullNext(out index);

                    KeyValuePair<int, GameObject> pair = new KeyValuePair<int, GameObject>(index, go);

                    objs.Add(pair);
                }

                return objs;
            }
            else
            {
                return null;
            }
        }

        public void Dispose()
        {
            foreach(var box in Boxes)
            {
                // free vuforia object
                // trigger vuforia pool disconnect
                // destroy the objects
                box.Dispose();
            }

            // Clean up any unchecked syllable planes
            cleanFloatingAssociatedSyllablePlanes();
        }

        private void cleanFloatingAssociatedSyllablePlanes()
        {
            // Search for any syllable plane objects that have no parent, and if their name matches up, destroy the syllable plane object
            var syllablePlaneScripts = GameObject.FindObjectsOfType<SyllablePlane>();
            foreach (var sps in syllablePlaneScripts)
            {
                var go = sps.gameObject;
                if(go.transform.parent != null)
                {
                    continue;
                }
                else
                {
                    bool matchedSyllable = false;

                    foreach(var s in shuffledSyllables_m)
                    {
                        if (s.Matches(sps.Syllable))
                        {
                            matchedSyllable = true;
                            break;
                        }
                    }

                    if (matchedSyllable)
                    {
#if UNITY_EDITOR
                        GameObject.DestroyImmediate(go);
#else
                        GameObject.Destroy(go);
#endif
                    }
                }
            }
        }
#endregion

#region Properties
        public Word Word
        {
            get
            {
                return word_m;
            }
        }
        public List<Syllable> ShuffledSyllables
        {
            get
            {
                return new List<Syllable>(shuffledSyllables_m);
            }
        }
#endregion
    }
}