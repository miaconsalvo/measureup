using System.Collections;
using System.Collections.Generic;
using Mystie.Dressup;
using UnityEngine;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Contestant Opinion", menuName = "Data/Opinions/Contestant Opinion")]
    public class ContestantCondition : Condition
    {
        public override Reaction Check(DressupManager dressup)
        {
            return dressup.reaction;
        }
    }
}
