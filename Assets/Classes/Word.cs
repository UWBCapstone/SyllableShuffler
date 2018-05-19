using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;

namespace SyllableShifter
{
    public class Word
    {
        private List<Syllable> syllables_m;

        public Word()
        {
            initializeVariables();
            syllables_m.Add(new Syllable(null));
        }

        public Word(IEnumerable<Syllable> syllables)
        {
            initializeVariables();
            syllables_m.AddRange(syllables);
        }

        public void Set(IEnumerable<Syllable> syllables)
        {
            syllables_m.Clear();
            syllables_m.AddRange(syllables);
        }

        public bool Matches(string word)
        {
            int index = 0;
            if(syllables_m == null
                || syllables_m.Count == 0)
            {
                return false;
            }

            for(int i = 0; i < syllables_m.Count; i++)
            {
                string component = word.Substring(index, word.Length - index);
                string syllable = syllables_m[i];
                if (!component.StartsWith(syllable))
                {
                    return false;
                }
            }

            return true;
        }

        public static implicit operator string(Word word)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var syllable in word.Syllables)
            {
                sb.Append(syllable);
            }

            return sb.ToString();
        }
        
        private void initializeVariables()
        {
            syllables_m = new List<Syllable>();
        }

        public bool IsValid()
        {
            return syllables_m != null && syllables_m.Count > 0;
        }

        #region Properties
        public List<Syllable> Syllables
        {
            get
            {
                return new List<Syllable>(syllables_m);
            }
        }

        public int SyllableCount
        {
            get
            {
                return syllables_m.Count;
            }
        }

        public int Length
        {
            get
            {
                int count = 0;
                foreach(var s in syllables_m)
                {
                    count += s.Length;
                }

                return count;
            }
        }
        public Syllable this[int index]
        {
            get
            {
                if (SyllableCount > index)
                {
                    Syllable orig = syllables_m[index];
                    Syllable copy = new Syllable(orig);
                    return copy;
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