using System.Collections;
using System.Collections.Generic;
using Mystie.Dressup;
using UnityEngine;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Opinion", menuName = "Data/Opinions/Rule Opinion")]
    public class RuleCondition : Condition
    {
        public TagsRuleData rule;

        public override Reaction Check(DressupManager dressup)
        {
            return rule.Check(dressup) ? Reaction.Positive : Reaction.Negative;
        }
    }
}
