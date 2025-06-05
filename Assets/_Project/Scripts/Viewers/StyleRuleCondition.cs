using System.Collections;
using System.Collections.Generic;
using Mystie.Dressup;
using UnityEngine;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Style Rule Opinion", menuName = "Data/Opinions/Style Rule Opinion")]
    public class StyleRuleCondition : Condition
    {
        public override Reaction Check(DressupManager dressup)
        {
            return dressup.CheckStyleRule() ? Reaction.Positive : Reaction.Negative;
        }
    }
}
