using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Actor Collection", menuName = "Visual Novel/Actor Collection", order = 0)]
    public class ActorCollection : ScriptableObject
    {
        public List<ActorItem> actors = new List<ActorItem>();

        [System.Serializable]
        public class ActorItem
        {
            public string name;
            public ActorScriptable character;
        }

        public Dictionary<string, ActorScriptable> Get()
        {
            Dictionary<string, ActorScriptable> dict = new Dictionary<string, ActorScriptable>();
            foreach (ActorItem c in actors)
            {
                dict.Add(c.name, c.character);
            }
            return dict;
        }
    }
}
