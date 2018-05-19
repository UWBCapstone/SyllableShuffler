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
        #endregion

        #region Methods
        #region Init
        public SyllableBoxSet(Word word)
        {
            bool shuffle = true;
            SetWord(word, shuffle);
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