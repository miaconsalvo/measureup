using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie
{
    [CreateAssetMenu(fileName = "ClothingKit", menuName = "Clothing/Clothing Kit", order = 3)]
    public class ClothingKitScriptable : ScriptableObject
    {
        public string name = "Clothing Kit";
        public int price = 400;
        public string priceRange = "$";
        public List<GarmentScriptable> garments = new List<GarmentScriptable>();
        public List<string> tags = new List<string>();
    }
}
