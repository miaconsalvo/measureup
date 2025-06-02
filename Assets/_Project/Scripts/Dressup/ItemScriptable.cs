using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Mystie.Dressup
{
    public enum ItemType { TOP, BOTTOM, SHOES }

    [CreateAssetMenu(fileName = "Item", menuName = "Data/Clothing/Item", order = 2)]
    public class ItemScriptable : ScriptableObject
    {
        public string name = "Item";
        [field: SerializeField] public ItemType type { get; private set; }
        public LocalizedString displayName;
        public LocalizedString description;
        public BrandScriptable brand;
        public Sprite icon;
        public Sprite sprite;
        public List<SpriteItem> sprites;

        public List<ClothingTag> tags = new List<ClothingTag>();

        public bool HasTag(string s)
        {
            foreach (ClothingTag tag in tags)
            {
                if (tag.name == s) return true;
            }
            return false;
        }
    }
}
