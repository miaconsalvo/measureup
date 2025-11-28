using Mystie.UI.Transition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie.UI.Transition
{
    [CreateAssetMenu(fileName = "Circle Transition", menuName = "Data/Scene Transitions/Circle")]
    public class CircleTransitionScriptable : AbstractSceneTransitionScriptable
    {
        public Sprite circleSprite;
        public Color color;

        public override IEnumerator Enter(Canvas parent)
        {
            InitImage(parent);

            float time = 0;
            float size = Mathf.Sqrt(
                Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2));
            Vector2 initialSize = new Vector2(size, size);
            while (time < 1)
            {
                animObj.rectTransform.sizeDelta = Vector2.Lerp(initialSize, Vector2.zero, time);
                yield return null;
                time += Time.deltaTime / animTime;
            }

            Destroy(animObj.gameObject);
        }

        public override IEnumerator Exit(Canvas parent)
        {
            InitImage(parent);

            float time = 0;
            float size = Mathf.Sqrt(
                Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2));
            Vector2 targetSize = new Vector2(size, size);
            while (time < 1)
            {
                animObj.rectTransform.sizeDelta = Vector2.Lerp(Vector2.zero, targetSize, time);
                yield return null;
                time += Time.deltaTime / animTime;
            }
        }

        protected override void InitImage(Canvas parent)
        {
            if (animObj != null) return;

            animObj = CreateImage(parent);
            animObj.color = color;
            animObj.rectTransform.sizeDelta = Vector2.zero;
            animObj.sprite = circleSprite;
        }
    }
}
