using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Mystie
{
    public abstract class EmailScriptable : ScriptableObject
    {
        public string sender;
        public abstract string Subject { get; }
        public abstract string Body { get; }
    }
}
