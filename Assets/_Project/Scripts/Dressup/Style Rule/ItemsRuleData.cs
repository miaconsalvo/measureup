using System.Collections.Generic;
using System.Linq;
using Mystie.Core;
using UnityEngine;

namespace Mystie.Dressup
{
    [CreateAssetMenu(fileName = "Item Rule", menuName = "Data/Items Rule", order = 3)]
    public class ItemsRuleData : StyleRuleData
    {
        [SerializeField] private List<ItemRule> rules;

        public override bool Check(DressupManager dressup)
        {
            List<ItemScriptable> equipped = dressup.items.Values
                .Where(item => item != null)
                .ToList(); ;
            Debug.Log($"Checking style rule {name}. Items present: {string.Join(", ", equipped.Select(t => t.name))}");

            return CheckRule(equipped);
        }

        public override bool Check(ItemScriptable item)
        {
            if (item == null) return false;
            return CheckRule(new List<ItemScriptable>() { item });
        }

        public bool CheckRule(List<ItemScriptable> items)
        {
            foreach (ItemRule rule in rules)
            {
                bool ruleResult = rule.Check(items);
                Debug.Log($"Rule check result: {ruleResult}");
                if (!ruleResult) return false;
            }

            return true;
        }
    }

    [System.Serializable]
    public class ItemRule
    {
        [SerializeField] private LogicOp op;
        [SerializeField] private bool not;
        [SerializeField] private List<ItemScriptable> items;

        public bool Check(List<ItemScriptable> equippedItems)
        {
            if (equippedItems == null) equippedItems = new List<ItemScriptable>();

            bool result = false;
            switch (op)
            {
                case LogicOp.AND:
                    result = true;
                    foreach (ItemScriptable item in items)
                        if (!equippedItems.Contains(item)) { result = false; break; }
                    break;
                case LogicOp.OR:
                    result = false;
                    foreach (ItemScriptable item in items)
                        if (equippedItems.Contains(item)) { result = true; break; }
                    break;
                case LogicOp.XOR:
                    result = false;
                    foreach (ItemScriptable item in items)
                    {
                        if (equippedItems.Contains(item))
                        {
                            if (!result) result = true;
                            else { result = false; break; }
                        }
                    }
                    break;
            }
            return result ^ not;
        }
    }
}
