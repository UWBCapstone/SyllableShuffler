using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter
{
    public class WordDropdownHandler : MonoBehaviour
    {
        #region Fields
        public WordDictionary wordDictionary;
        public UnityEngine.UI.Dropdown wordDropdown;
        private Dictionary<int, string> indexToWordMap;
        #endregion

        #region Methods
        public void Start()
        {
            indexToWordMap = new Dictionary<int, string>();
        }

        public void Awake()
        {
            wordDropdown.ClearOptions();
            wordDropdown.AddOptions(wordDictionary.AvailableWords);
            fillIndexToWordMap();
        }

        public string GetWordFromDropdownIndex(int index)
        {
            if (indexToWordMap.ContainsKey(index))
            {
                return indexToWordMap[index];
            }

            return null;
        }

        private void fillIndexToWordMap()
        {
            var words = wordDictionary.AvailableWords;
            for(int i = 0; i < words.Count; i++)
            {
                indexToWordMap.Add(i, words[i]);
            }
        }
        #endregion

        #region Properties
        public List<string> availableWords
        {
            get
            {
                return wordDictionary.AvailableWords;
            }
        }
        #endregion
    }
}