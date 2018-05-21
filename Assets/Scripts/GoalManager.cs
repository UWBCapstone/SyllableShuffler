using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter.Scripts
{
    public class GoalManager : MonoBehaviour
    {
        public bool SwitchColors = true;
        public Color CorrectColor = Color.green;
        public Color IncorrectColor = Color.red;

        public WordGenerator wordGenerator;

        public void Update()
        {
            if (SwitchColors)
            {
                SetCorrectnessColors(wordGenerator.BoxSets);
            }
            if (AllCorrect())
            {
                Debug.Log("Boxes in correct order!");
            }
        }

        public bool AllCorrect()
        {
            // safeguard
            if(wordGenerator == null
                || wordGenerator.BoxSets == null
                || wordGenerator.BoxSets.Count <= 0)
            {
                return false;
            }
            
            foreach(var set in wordGenerator.BoxSets)
            {
                if (!IsCorrect(set))
                {
                    return false;
                }
            }

            return true;
        }

        public void SetCorrectnessColors(List<SyllableBoxSet> sets)
        {
            foreach(var set in sets)
            {
                if (!IsCorrect(set))
                {
                    set.SetColor(IncorrectColor);
                }
                else
                {
                    set.SetColor(CorrectColor);
                }
            }
        }

        public bool IsCorrect(SyllableBoxSet set)
        {
            if (set != null)
            {
                Syllable[] syllablesInVisualOrder = GetSyllableOrder(set);
                if (syllablesInVisualOrder != null)
                {
                    return set.IsCorrectOrder(syllablesInVisualOrder);
                }
            }

            return false;
        }

        public Syllable[] GetSyllableOrder(SyllableBoxSet set)
        {
            Vector3 avgForward;
            if(allBoxesFacingSameDirection(set, out avgForward))
            {
                return getSyllablesInVisualOrder(set);
            }
            else
            {
                return null;
            }
        }

        private bool allBoxesFacingSameDirection(SyllableBoxSet set, out Vector3 avgDirection)
        {
            if (set != null
                && set.Count > 0)
            {
                Vector3 avgVector = set[0].gameObject.transform.forward;
                for (int i = 1; i < set.Count; i++)
                {
                    Vector3 x = set[i].gameObject.transform.forward;
                    avgVector = (x + avgVector) / (i+1);

                    float dot = Vector3.Dot(avgVector, x);
                    if(dot <= 0)
                    {
                        avgDirection = avgVector;
                        return false;
                    }
                }

                avgDirection = avgVector;
                return true;
            }

            avgDirection = Vector3.zero;
            return false;
        }

        private Syllable[] getSyllablesInVisualOrder(SyllableBoxSet set)
        {
            List<SyllableBox> boxList = new List<SyllableBox>(set.Boxes);
            if(boxList.Count > 1)
            {
                VisualBoxComparator vbc = new VisualBoxComparator();
                boxList.Sort(vbc);
            }

            List<Syllable> syllablesInVisualOrderList = new List<Syllable>();
            for(int i = 0; i < boxList.Count; i++)
            {
                syllablesInVisualOrderList.Add(boxList[i].Syllable);
            }

            return syllablesInVisualOrderList.ToArray();
        }
    }
}