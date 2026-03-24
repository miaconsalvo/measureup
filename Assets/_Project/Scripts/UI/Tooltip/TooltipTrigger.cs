using System;
using System.Collections;
using DG.Tweening;
using Mystie.Core;
using Mystie.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mystie
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        public string header;
        [TextArea] public string content;

        public enum TooltipPositioningType
        {
            mousePosition,
            mousePositionAndFollow,
            transformPosition
        }

        [Tooltip("Defines where the tooltip will be placed and how that placement will occur. Transform position will always be used if this element wasn't selected via mouse")]
        public TooltipPositioningType positioningType = TooltipPositioningType.mousePosition;
        public Vector2 offset;
        public float delayTime = 0.5f;

        private bool hovered = false;
        private Vector2 tooltipPos;
        private TooltipUI tooltipUI;

        public Vector2 GetPos(Vector2 pointerPos)
        {
            switch (positioningType)
            {
                case TooltipPositioningType.mousePositionAndFollow:
                    return (tooltipPos = pointerPos + offset);
                case TooltipPositioningType.mousePosition:
                case TooltipPositioningType.transformPosition:
                default:
                    return tooltipPos;
            }
        }

        public void HideTooltip()
        {
            TooltipSystem.HideTooltip(); //tooltipUI
            hovered = false;
            tooltipUI = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (hovered || tooltipUI != null
                || (String.IsNullOrWhiteSpace(header) && String.IsNullOrWhiteSpace(content)))
                return;

            switch (positioningType)
            {
                case TooltipPositioningType.mousePosition:
                case TooltipPositioningType.mousePositionAndFollow:
                    tooltipPos = eventData.position + offset;
                    break;
                case TooltipPositioningType.transformPosition:
                    tooltipPos = (Vector2)transform.position + offset;
                    break;
            }

            hovered = true;
            tooltipUI = TooltipSystem.ShowTooltip(tooltipPos, delayTime, content, header);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!hovered || tooltipUI == null) return;
            HideTooltip();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            TooltipSystem.SetPosition(GetPos(eventData.position));
        }
    }
}
