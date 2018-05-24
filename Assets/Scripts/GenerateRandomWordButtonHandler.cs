using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter
{
    public class GenerateRandomWordButtonHandler : MonoBehaviour
    {
        public UnityEngine.UI.Button randomWordGeneratorButton;
        public WordGenerator wordGenerator;

        public void OnClick()
        {
            wordGenerator.GenerateRandom();
        }
    }
}