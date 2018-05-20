using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq; // For getting the Enumerable class which is used to grab a random word

using System.Linq;

namespace SyllableShifter
{
    public class WordDictionary : MonoBehaviour
    {
        #region Fields
        public Dictionary<string, List<Syllable>> wordDictionary_m;
        private int maxSyllableCount_m;
        private int minSyllableCount_m;
        #endregion

        #region Methods
        #region Initialization
        public WordDictionary()
        {
            wordDictionary_m = new Dictionary<string, List<Syllable>>();
            AddDefaultDictionaryWords();
            maxSyllableCount_m = 0;
            minSyllableCount_m = int.MaxValue;
        }

        public void AddDefaultDictionaryWords()
        {
            string[] strings = new string[]
            {
                "Procedure",
                "Decided",
                "Example",
                "Directly",
                "Important",
                "Consider",
                "Completely",
                "Advantage",
                "Agenda",
                "Mistaken",
                "Forgetful",
                "Exactly",
                "Iconic",
                "Nutrition",
                "Unwieldy"
            };
            
            Word procedure = new Word(new string[] { "pro", "ce", "dure" });
            Word decided = new Word(new string[] { "de", "ci", "ded" });
            Word example = new Word(new string[] { "ex", "am", "ple" });
            Word directly = new Word(new string[] { "dir", "ect", "ly" });
            Word important = new Word(new string[] { "im", "por", "tant" });
            Word consider = new Word(new string[] { "con", "si", "der" });
            Word completely = new Word(new string[] { "com", "plete", "ly" });
            Word advantage = new Word(new string[] { "ad", "van", "tage" });
            Word agenda = new Word(new string[] { "a", "gen", "da" });
            Word mistaken = new Word(new string[] { "mis", "tak", "en" });
            Word forgetful = new Word(new string[] { "for", "get", "ful" });
            Word exactly = new Word(new string[] { "ex", "act", "ly" });
            Word iconic = new Word(new string[] { "i", "con", "ic" });
            Word nutrition = new Word(new string[] { "nu", "tri", "tion" });
            Word unwieldy = new Word(new string[] { "un", "wiel", "dy" });

            List<Word> words = new List<Word>();
            words.Add(procedure);
            words.Add(decided);
            words.Add(example);
            words.Add(directly);
            words.Add(important);
            words.Add(consider);
            words.Add(completely);
            words.Add(advantage);
            words.Add(agenda);
            words.Add(mistaken);
            words.Add(forgetful);
            words.Add(exactly);
            words.Add(iconic);
            words.Add(nutrition);
            words.Add(unwieldy);

            if(words.Count != strings.Length)
            {
                Debug.LogError("Default of initialization of word dictionary encountered mismatching lengths of word vs syllable lists");
            }
            else
            {
                for(int i = 0; i < strings.Length; i++)
                {
                    //List<Syllable> syllables = new List<Syllable>(words[i].Syllables);
                    //Add(strings[i].ToLower(), syllables);
                    Add(strings[i].ToLower(), words[i].Syllables);
                }
            }
        }
        #endregion

        #region Public
        public Word GetRandomWord()
        {
            if(wordDictionary_m != null
                && wordDictionary_m.Count > 0)
            {
                System.Random rand = new System.Random();
                List<string> keys = Enumerable.ToList<string>(wordDictionary_m.Keys);
                int index = rand.Next(keys.Count);
                return new Word(wordDictionary_m[keys[index]]);
            }
            else
            {
                return null;
            }
        }

        public Word GetWord(string word)
        {
            word = word.ToLower();
            if (wordDictionary_m.ContainsKey(word))
            {
                return new Word(wordDictionary_m[word]);
            }
            else
            {
                return null;
            }
        }

        public List<Word> GetWordsWithSyllableCount(int count)
        {
            if(count < minSyllableCount_m
                || count > maxSyllableCount_m)
            {
                return null;
            }

            List<Word> words = new List<Word>();
            foreach(var key in wordDictionary_m.Keys)
            {
                string wordStr = key;
                List<Syllable> syllables = wordDictionary_m[key];
                if(syllables.Count == count)
                {
                    Word newWord = new Word(syllables);
                    words.Add(newWord);
                }
            }

            return words;
        }

        public void Add(string word, List<Syllable> syllables)
        {
            word = word.ToLower();
            if (!wordDictionary_m.ContainsKey(word)) {
                wordDictionary_m.Add(word, syllables);

                if(syllables.Count > maxSyllableCount_m)
                {
                    maxSyllableCount_m = syllables.Count;
                }
                if(syllables.Count < minSyllableCount_m)
                {
                    minSyllableCount_m = syllables.Count;
                }
            }
        }

        public void Remove(string word)
        {
            word = word.ToLower();
            if (wordDictionary_m.ContainsKey(word))
            {
                int syllableCount = wordDictionary_m[word].Count;
                wordDictionary_m.Remove(word);

                if (wordDictionary_m.Count <= 0)
                {
                    minSyllableCount_m = int.MaxValue;
                    maxSyllableCount_m = 0;
                }
                else
                {
                    if (syllableCount == minSyllableCount_m)
                    {
                        minSyllableCount_m = checkMinSyllableCount();
                    }
                    if (syllableCount == maxSyllableCount_m)
                    {
                        maxSyllableCount_m = checkMaxSyllableCount();
                    }
                }
            }
        }

        public void SetSyllables(string word, List<Syllable> syllables)
        {
            word = word.ToLower();
            if (wordDictionary_m.ContainsKey(word))
            {
                Remove(word);
            }
            Add(word, syllables);
        }
        #endregion

        #region Helpers
        private int checkMinSyllableCount()
        {
            int min = int.MaxValue;
            foreach(var key in wordDictionary_m.Keys)
            {
                if(wordDictionary_m[key].Count < min)
                {
                    min = wordDictionary_m[key].Count;
                }
            }

            if (min.Equals(int.MaxValue))
            {
                min = 0;
            }

            return min;
        }

        private int checkMaxSyllableCount()
        {
            int max = 0;
            foreach (var key in wordDictionary_m.Keys)
            {
                if(wordDictionary_m[key].Count > max)
                {
                    max = wordDictionary_m[key].Count;
                }
            }

            return max;
        }
        #endregion
        #endregion

        #region Properties
        public List<Syllable> this[string word]
        {
            get
            {
                if (wordDictionary_m.ContainsKey(word))
                {
                    List<Syllable> syllables = new List<Syllable>(wordDictionary_m[word]);
                    return syllables;
                }
                else
                {
                    return null;
                }
            }
        }
        public int Count
        {
            get
            {
                return wordDictionary_m.Count;
            }
        }
        public bool Empty
        {
            get
            {
                return wordDictionary_m.Count == 0;
            }
        }
        public List<string> AvailableWords
        {
            get
            {
                return Enumerable.ToList<string>(wordDictionary_m.Keys);
            }
        }
        /// <summary>
        /// Warning: Very slow. Use sparingly.
        /// </summary>
        public int[] AvailableSyllableCounts
        {
            get
            {
                if (maxSyllableCount_m > minSyllableCount_m)
                {
                    List<int> potentialSyllableCounts = new List<int>(Enumerable.Range(minSyllableCount_m, (maxSyllableCount_m - minSyllableCount_m)));
                    List<int> availableSyllables = new List<int>();
                    for (int i = 0; i < potentialSyllableCounts.Count; i++)
                    {
                        int syllableCount = potentialSyllableCounts[i];
                        bool countEncountered = false;

                        foreach (var v in wordDictionary_m.Values)
                        {
                            if (v.Count == syllableCount)
                            {
                                countEncountered = true;
                                break;
                            }
                        }

                        if (countEncountered)
                        {
                            availableSyllables.Add(syllableCount);
                        }
                    }

                    return availableSyllables.ToArray();
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