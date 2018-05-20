using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace SyllableShifter
{
    [CustomEditor(typeof(WordGenerator))]
    public class WordGeneratorEditor : Editor
    {
        int wordChoice = 0;
        int numSyllableChoice = 0;
        
        List<string> availableSyllableCountStrings = new List<string>();
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            WordGenerator generator = (WordGenerator)target;
            List<string> availableWords = generator.wordDictionary_m.AvailableWords;

            EditorGUILayout.HelpBox("The available words stored in the dictionary are shown below...", MessageType.Info);
            wordChoice = EditorGUILayout.Popup(wordChoice, availableWords.ToArray());
            generator.wordToGenerate = availableWords[wordChoice];

            if(GUILayout.Button("Generate Selected Word"))
            {
                generator.Generate(generator.wordToGenerate);
            }
            if(GUILayout.Button("Generate Random Word"))
            {
                generator.GenerateRandom();
            }

            EditorGUILayout.HelpBox("You can generate a random word by selecting the number of syllables for the word below...", MessageType.Info);
            var availableSyllableCounts = generator.wordDictionary_m.AvailableSyllableCounts;
            if (availableSyllableCounts != null)
            {
                foreach (var n in availableSyllableCounts)
                {
                    if (!availableSyllableCountStrings.Contains(n.ToString()))
                    {
                        availableSyllableCountStrings.Add(n.ToString());
                    }
                }
            }
            numSyllableChoice = EditorGUILayout.Popup(numSyllableChoice, availableSyllableCountStrings.ToArray());

            if(GUILayout.Button("Generate Word With Chosen # of Syllables"))
            {
                generator.GenerateRandom(int.Parse(availableSyllableCountStrings[numSyllableChoice]));
            }
        }
    }
}