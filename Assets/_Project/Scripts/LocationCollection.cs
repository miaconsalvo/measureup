using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Location Collection", menuName = "Visual Novel/Location Collection", order = 2)]
    public class LocationCollection : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<string, GameObject> locations;

        public Dictionary<string, GameObject> Instantiate(Transform parent)
        {
            Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();
            foreach (string location in locations.Keys)
            {
                GameObject item = Instantiate(locations[location], parent);
                item.SetActive(false);
                dict.Add(location, item);
            }
            return dict;
        }
    }
}
