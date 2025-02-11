using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie.Dressup
{
    [CreateAssetMenu(fileName = "ClothingKit", menuName = "Clothing/Clothing Kit", order = 3)]
    public class ClothingKitScriptable : ScriptableObject
    {
        public string name = "Clothing Kit";
        public int price = 400;
        public string priceRange = "$";
        public List<GarmentScriptable> garments = new List<GarmentScriptable>();
        public List<ClothingTag> tags = new List<ClothingTag>();

        public List<string> Tags {
            get {
                List<string> t = new List<string>();
                foreach(ClothingTag tag in tags) t.Add(tag.name);
                return t;
            }
        }
    }
}
