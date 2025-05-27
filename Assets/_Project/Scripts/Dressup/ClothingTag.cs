using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Mystie.Dressup
{
    [CreateAssetMenu(fileName = "ClothingTag", menuName = "Clothing/Tag", order = 5)]
    public class ClothingTag : ScriptableObject
    {
        public string name;
        public LocalizedString displayName;
        public Color color;
        public Sprite sprite;
        public enum TagType { Special, Type, Style, Color }
        public TagType type;

        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower();
            }
        }
    }
}
