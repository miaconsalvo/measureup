using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Email", menuName = "Data/Email/Standard Email", order = 0)]
    public class StandardEmailScriptable : EmailScriptable
    {
        [SerializeField] private LocalizedString subject;
        [SerializeField] private LocalizedString body;

        public override string Subject => subject.GetLocalizedString();
        public override string Body => body.GetLocalizedString();
    }
}
