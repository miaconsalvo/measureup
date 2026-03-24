using System.Collections;
using System.Collections.Generic;
using Mystie.Core;
using Mystie.UI;
using UnityEngine;
using UnityEngine.Pool;

namespace Mystie.UI
{
    public class TooltipSystem : MonoBehaviour
    {
        private static TooltipSystem instance;

        public RectTransform tooltipAnchor;
        public TooltipUI tooltip;
        public TooltipUI tooltipPrefab;
        public float delayTime = 0.5f;
        public Vector2 offset = new Vector2(0, 0);

        private Stack<TooltipUI> tooltipStack;
        private ObjectPool<TooltipUI> tooltipPool;
        private Coroutine showDelay;

        public void Awake()
        {
            instance = this;
            tooltipStack = new Stack<TooltipUI>();
            tooltipPool = new ObjectPool<TooltipUI>(
                createFunc: InstantiateTooltip,
                actionOnGet: GetTooltip,
                actionOnRelease: HideTooltip,
                collectionCheck: true,
                defaultCapacity: 1,
                maxSize: 10
            );
        }

        public static TooltipUI ShowTooltip(Vector2 pos, string tooltipText, string tooltipHeader = "")
        {
            return ShowTooltip(pos, instance.delayTime, tooltipText, tooltipHeader);
        }

        public static TooltipUI ShowTooltip(Vector2 pos, float delayTime, string tooltipText, string tooltipHeader = "")
        {
            instance.tooltip = instance.tooltipPool.Get();
            instance.tooltipStack.Push(instance.tooltip);

            if (instance.tooltip != null)
            {
                instance.tooltip?.SetText(tooltipText, tooltipHeader);
                SetPosition(instance.tooltip, pos);

                instance.showDelay = instance.StartCoroutine(instance.ShowRoutine(instance.tooltip));
            }

            return instance.tooltip;
        }

        public IEnumerator ShowRoutine(TooltipUI tooltip)
        {
            yield return new WaitForSecondsRealtime(instance.delayTime);

            tooltip?.Show();
        }

        public static void HideTooltip() //TooltipUI tooltip
        {
            if (instance.tooltipStack.IsNullOrEmpty())
                return;

            if (instance.showDelay != null)
                instance.StopCoroutine(instance.showDelay);

            TooltipUI tooltip = instance.tooltipStack.Pop();
            tooltip.Hide();

            instance.tooltipPool.Release(tooltip);

            instance.tooltipStack.TryPeek(out instance.tooltip);
        }

        #region Position

        public static void SetPosition(Vector2 newPos)
        {
            if (instance.tooltip == null) return;
            SetPosition(instance.tooltip, newPos);
        }

        public static void SetPosition(TooltipUI tooltip, Vector2 newPos)
        {
            if (tooltip == null) return;

            Vector2 normalizedPivot = new Vector2(newPos.x / Screen.width, newPos.y / Screen.height);

            tooltip.rectTransform.pivot = CalculatePivot(normalizedPivot);
            tooltip.transform.position = newPos + instance.offset;
        }

        private static Vector2 CalculatePivot(Vector2 normalizedPos)
        {
            Vector2 finalPivot = Vector2.zero;

            if (normalizedPos.x < 0.5f) finalPivot.x = -0.1f;
            else finalPivot.x = 1.01f;

            if (normalizedPos.y < 0.5f) finalPivot.y = -0.1f;
            else finalPivot.y = 1.01f;

            return finalPivot;
        }

        #endregion

        #region Pool

        public TooltipUI InstantiateTooltip()
        {
            TooltipUI tooltip = Instantiate(tooltipPrefab, tooltipAnchor);
            tooltip.gameObject.name = "TooltipUI";
            tooltip.gameObject.SetActive(false);
            return tooltip;
        }

        public void GetTooltip(TooltipUI tooltip)
        {
            tooltip.gameObject.SetActive(true);
        }

        public void HideTooltip(TooltipUI tooltip)
        {
            tooltip.Hide();
            tooltip.gameObject.SetActive(false);
        }

        #endregion
    }
}
