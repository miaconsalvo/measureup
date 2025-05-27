using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Brand", menuName = "Clothing/Brand", order = 4)]
    public class BrandScriptable : ScriptableObject
    {
        public string name = "Brand";
        public Sprite logo;
    }
}
