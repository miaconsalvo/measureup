using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Localization;

namespace Mystie.Dressup
{
    [CreateAssetMenu(fileName = "Contestant", menuName = "Data/Contestant", order = 1)]
    public class ContestantData : ScriptableObject
    {
        public string name;

        public string age;
        public LocalizedString occupation;

        [field: Space]

        [field: SerializeField] public Sprite model { get; private set; }
        public ActorScriptable actor;
        public List<ItemScriptable> startingClothes;
        public List<ItemScriptable> underwear;

        [Space]

        public List<ClothingTag> positiveTags = new List<ClothingTag>();
        public List<ClothingTag> negativeTags = new List<ClothingTag>();

        [field: SerializeField] public LocalizedString opinionDefault { get; private set; }
        public List<OpinionCondition> opinionConditions = new List<OpinionCondition>();

        private void OnValidate()
        {
            startingClothes = startingClothes
                .GroupBy(item => item.type)
                .Select(group => group.First())
                .ToList();

            underwear = underwear
                .GroupBy(item => item.type)
                .Select(group => group.First())
                .ToList();
        }
    }

    [System.Serializable]
    public class OpinionCondition
    {
        public string name = "Opinion";
        public List<ClothingTag> requiredTags = new List<ClothingTag>();
        public List<ClothingTag> excludedTags = new List<ClothingTag>();
        public int minNegativeTags = 0;
        public int minPositiveTags = 0;
        //public List<string> opinions = new List<string>();
        public List<LocalizedString> opinions = new List<LocalizedString>();
    }
}
