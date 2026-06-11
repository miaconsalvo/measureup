using System.Collections.Generic;
using Mystie.Core;
using UnityEngine;
using UnityEngine.Localization;

namespace Mystie.Dressup
{
    public abstract class StyleRuleData : ScriptableObject
    {
        public LocalizedString ruleName;
        public LocalizedString ruleDescription;

        public abstract bool Check(DressupManager dressup);

        public abstract bool Check(ItemScriptable item);
    }


}
