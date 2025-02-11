using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie.Dressup
{
    public enum GarmentType{TOP, BOTTOM, SHOES}

    [CreateAssetMenu(fileName = "Garment", menuName = "Clothing/Garment", order = 2)]
    public class GarmentScriptable : ScriptableObject
    {
        public string name = "Garment";
        public Sprite icon;
        public Sprite sprite;

        [field: SerializeField] public GarmentType type {get; private set;}
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
