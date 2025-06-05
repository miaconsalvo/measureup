using System.Collections;
using System.Collections.Generic;
using Mystie.Core;
using UnityEngine;
using UnityEngine.Localization;

namespace Mystie.Dressup
{
    [CreateAssetMenu(fileName = "Style Rule", menuName = "Data/Style Rule", order = 2)]
    public class StyleRuleScriptable : ScriptableObject
    {
        public LocalizedString ruleName;
        public LocalizedString ruleDescription;
        [SerializeField] private List<TagRule> rules;

        public bool Check(List<ClothingTag> tags)
        {
            foreach (TagRule rule in rules)
            {
                if (!rule.Check(tags)) return false;
            }

            return true;
        }
    }

    [System.Serializable]
    public class TagRule
    {
        [SerializeField] private LogicOp op;
        [SerializeField] private bool not;
        [SerializeField] private List<ClothingTag> tags;

        public bool Check(List<ClothingTag> tagsList)
        {
            bool result = false;

            switch (op)
            {
                case LogicOp.AND:
                    result = true;
                    foreach (ClothingTag tag in tags)
                    {
                        if (!tagsList.Contains(tag))
                        {
                            result = false;
                            break;
                        }
                    }
                    break;
                case LogicOp.OR:
                    result = false;
                    foreach (ClothingTag tag in tags)
                    {
                        if (tagsList.Contains(tag))
                        {
                            result = true;
                            break;
                        }
                    }
                    break;
                case LogicOp.XOR:
                    result = false;
                    foreach (ClothingTag tag in tags)
                    {
                        if (tagsList.Contains(tag))
                        {
                            if (result == false) result = true;
                            else if (result == true)
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                    break;
            }

            return result ^ not;
        }
    }
}
