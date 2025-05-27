using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Location Collection", menuName = "Visual Novel/Location Collection", order = 2)]
    public class LocationCollection : ScriptableObject
    {
        [SerializeField] private List<Location> locations = new List<Location>();

        [System.Serializable]
        public class Location
        {
            public string name;
            public GameObject location;
        }

        public Dictionary<string, GameObject> Get(Transform parent)
        {
            Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();
            foreach (Location item in locations)
            {
                GameObject location = Instantiate(item.location, parent);
                location.SetActive(false);
                dict.Add(item.name, location);
            }
            return dict;
        }
    }
}
