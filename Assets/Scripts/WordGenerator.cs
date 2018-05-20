using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter
{
    public class WordGenerator : MonoBehaviour
    {
        #region Fields
        [HideInInspector]
        public string wordToGenerate = "Procedure";

        public WordDictionary wordDictionary_m;
        private List<SyllableBoxSet> boxSets_m;
        private System.Random rand_m;
        #endregion

        #region Methods
        public void Start()
        {
            boxSets_m = new List<SyllableBoxSet>();
            rand_m = new System.Random();
        }

        public void Generate(string wordStr)
        {
            // if the word exists in the Dictionary, generate the blocks based off of this
            Word word = wordDictionary_m.GetWord(wordStr);
            SyllableBoxSet boxSet = new SyllableBoxSet(word);
            boxSets_m.Add(boxSet);
        }

        public void GenerateRandom(int syllableCount)
        { 
            List<Word> options = wordDictionary_m.GetWordsWithSyllableCount(syllableCount);
            Word word = options[rand_m.Next(options.Count)];
            SyllableBoxSet boxSet = new SyllableBoxSet(word);
            boxSets_m.Add(boxSet);
        }

        public void GenerateRandom()
        {
            Word word = wordDictionary_m.GetRandomWord();
            SyllableBoxSet boxSet = new SyllableBoxSet(word);
            boxSets_m.Add(boxSet);
        }

        public void Clear()
        {
            foreach(var set in boxSets_m)
            {
                set.Dispose();
            }
        }
        #endregion
    }
}