using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Mystie.Dressup
{
    [CreateAssetMenu(fileName = "ClothingKit", menuName = "Data/Clothing/Clothing Kit", order = 3)]
    public class ClothingKitScriptable : ScriptableObject
    {
        public string name = "Clothing Kit";
        public LocalizedString displayName;
        public int price = 400;
        public string priceRange = "$";
        public List<ItemScriptable> garments = new List<ItemScriptable>();
        public List<ClothingTag> tags = new List<ClothingTag>();

        public List<LocalizedString> Tags
        {
            get
            {
                List<LocalizedString> t = new List<LocalizedString>();
                foreach (ClothingTag tag in tags) t.Add(tag.displayName);
                return t;
            }
        }
    }
}
