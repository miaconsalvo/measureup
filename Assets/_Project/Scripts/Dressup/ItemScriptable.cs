using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Mystie.Dressup
{
    public enum GarmentType { TOP, BOTTOM, SHOES }

    [CreateAssetMenu(fileName = "Item", menuName = "Clothing/Item", order = 2)]
    public class ItemScriptable : ScriptableObject
    {
        public string name = "Item";
        public LocalizedString displayName;
        public LocalizedString description;
        public BrandScriptable brand;
        public Sprite icon;
        public Sprite sprite;

        [field: SerializeField] public GarmentType type { get; private set; }
        public List<ClothingTag> tags = new List<ClothingTag>();
    }
}
