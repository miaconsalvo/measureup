using Mystie.UI.Transition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie.UI.Transition
{
    [CreateAssetMenu(fileName = "Fade Transition", menuName = "Data/Scene Transitions/Fade")]
    public class FadeTransitionScriptable : AbstractSceneTransitionScriptable
    {
        public Color color = Color.black;

        public override IEnumerator Enter(Canvas parent)
        {
            InitImage(parent);

            float time = 0;
            Color startColor = color;
            Color endColor = new Color(0, 0, 0, 0);
            while (time < 1)
            {
                animObj.color = Color.Lerp(startColor, endColor, time);
                yield return null;
                time += Time.deltaTime / animTime;
            }

            Destroy(animObj.gameObject);
        }

        public override IEnumerator Exit(Canvas parent)
        {
            InitImage(parent);

            float time = 0;
            Color startColor = new Color(0, 0, 0, 0);
            Color endColor = color;
            while (time < 1)
            {
                animObj.color = Color.Lerp(startColor, endColor, time);
                yield return null;
                time += Time.deltaTime / animTime;
            }
        }

        protected override void InitImage(Canvas parent)
        {
            if (animObj != null) return;

            animObj = CreateImage(parent);
            animObj.rectTransform.anchorMin = Vector2.zero;
            animObj.rectTransform.anchorMax = Vector2.one;
            animObj.rectTransform.sizeDelta = Vector2.zero;
        }
    }
}
