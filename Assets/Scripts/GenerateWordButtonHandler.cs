using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter
{
    public class GenerateWordButtonHandler : MonoBehaviour
    {
        public UnityEngine.UI.Button generateWordButton;
        public UnityEngine.UI.Dropdown wordDropdown;
        public WordDropdownHandler wordDropDownHandler;
        public WordGenerator wordGenerator;
        
        public void OnClick()
        {
            int index = wordDropdown.value;
            string wordStr = wordDropDownHandler.GetWordFromDropdownIndex(index);
            wordGenerator.wordToGenerate = wordStr;
            wordGenerator.Generate(wordStr);
        }
    }
}