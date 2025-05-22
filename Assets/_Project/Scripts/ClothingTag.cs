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
    }
}
