using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Garment", menuName = "Clothing/Garment", order = 2)]
    public class GarmentScriptable : ScriptableObject
    {
        public string name = "Garment";
        public Sprite icon;
        public Sprite sprite;

        public enum GarmentType{TOP, BOTTOM, SHOES}
        [SerializeField] private GarmentType type;
    }
}
