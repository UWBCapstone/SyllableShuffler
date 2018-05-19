using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyllableShifter
{
    public class Syllable
    {
        #region Fields
        private string text_m;
        private uint length_m;

        private static readonly string defaultText_m = "_NULL";
        #endregion

        public Syllable()
        {
            text_m = defaultText_m;
            length_m = 0;
        }

        public Syllable(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                text_m = text;
                length_m = (uint)text.Length;
            }
            else
            {
                text_m = defaultText_m;
                length_m = 0;
            }
        }

        public bool Matches(Syllable other)
        {
            if (other != null)
            {
                return other.Text.ToLower().Equals(Text.ToLower());
            }
            else
            {
                return false;
            }
        }

        public bool IsValid()
        {
            return length_m > 0;
        }

        public static implicit operator string(Syllable syllable)
        {
            if(syllable == null)
            {
                return null;
            }
            else if (syllable.IsValid())
            {
                return syllable.Text;
            }
            else
            {
                return string.Empty;
            }
        }

        #region Properties
        public string Text
        {
            get
            {
                return text_m;
            }
            set
            {
                text_m = value;
            }
        }

        public int Length
        {
            get
            {
                return (int)length_m;
            }
        }
        #endregion
    }
}