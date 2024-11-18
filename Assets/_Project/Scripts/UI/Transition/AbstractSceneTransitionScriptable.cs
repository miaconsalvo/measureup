using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.UI.Transition
{
    public abstract class AbstractSceneTransitionScriptable : ScriptableObject
    {
        public float animTime = 0.25f;
        protected Image animObj;

        public abstract IEnumerator Enter(Canvas parent);
        public abstract IEnumerator Exit(Canvas parent);

        protected virtual Image CreateImage(Canvas parent)
        {
            GameObject child = new GameObject("Transition Panel");
            child.transform.SetParent(parent.transform, false);

            return child.AddComponent<Image>();
        }

        protected abstract void InitImage(Canvas parent);
    }
}
