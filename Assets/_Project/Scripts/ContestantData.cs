using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie.Dressup
{
    [CreateAssetMenu(fileName = "Contestant", menuName = "Clothing/Contestant", order = 1)]
    public class ContestantData : ScriptableObject
    {
        public List<ClothingTag> positiveTags = new List<ClothingTag>();
        public List<ClothingTag> negativeTags = new List<ClothingTag>();

        public List<OpinionCondition> opinionConditions = new List<OpinionCondition>();
    }

    [System.Serializable]
        public class OpinionCondition
        {
            public List<ClothingTag> requiredTags = new List<ClothingTag>();
            public List<ClothingTag> excludedTags = new List<ClothingTag>();
            public int minNegativeTags = 0;
            public int minPositiveTags = 0;
            public List<string> opinions = new List<string>();
        }
}
