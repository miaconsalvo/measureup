using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mystie.Core;
using UnityEngine;
using UnityEngine.Localization;

namespace Mystie.Dressup
{
    [CreateAssetMenu(fileName = "Tags Rule", menuName = "Data/Tags Rule", order = 2)]
    public class TagsRuleData : StyleRuleData
    {
        [SerializeField] private List<TagRule> rules;

        public override bool Check(DressupManager dressup)
        {
            List<ClothingTag> tags = dressup.currentTags;
            Debug.Log($"Checking style rule {name}. Tags present: {string.Join(", ", tags.Select(t => t.name))}");

            return CheckRule(tags);
        }

        public override bool Check(ItemScriptable item)
        {
            if (item == null) return false;
            return CheckRule(LevelManager.Instance.GetEffectiveTags(item));
        }

        public bool CheckRule(List<ClothingTag> tags)
        {
            foreach (TagRule rule in rules)
            {
                bool ruleResult = rule.Check(tags);
                Debug.Log($"Rule check result: {ruleResult}");
                if (!ruleResult) return false;
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
        [SerializeField] private ItemScriptable item;

        public bool Check(List<ClothingTag> tagsList)
        {
            bool result = false;

            switch (op)
            {
                case LogicOp.AND:
                    if (tagsList.IsNullOrEmpty())
                        return not ? true : tags.IsNullOrEmpty();

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
                    if (tagsList.IsNullOrEmpty())
                        return not ? true : tags.IsNullOrEmpty();

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
                    if (tagsList.IsNullOrEmpty())
                        return not;

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
