using System.Collections;
using System.Collections.Generic;
using Mystie.Dressup;
using UnityEngine;

namespace Mystie
{
    public abstract class Condition : ScriptableObject
    {
        public abstract Reaction Check(DressupManager dressup);
    }
}
