using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie.Core
{
    internal static class GameObjectExtension
    {
        public static bool IsInLayerMask(this GameObject gameObject, LayerMask mask)
        {
            return (mask == (mask | (1 << gameObject.layer)));
        }
    }
}
