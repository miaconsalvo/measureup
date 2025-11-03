using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Actor Collection", menuName = "Visual Novel/Actor Collection", order = 0)]
    public class ActorCollection : ScriptableObject
    {
        [field: SerializeField] public SerializedDictionary<string, ActorScriptable> actors { get; private set; }
    }
}
