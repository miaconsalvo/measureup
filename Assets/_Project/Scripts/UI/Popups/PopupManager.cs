using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Mystie.UI
{
    public class PopupManager : MonoBehaviour
    {
        [SerializeField] private RectTransform popupAnchor;
        [SerializeField] private ConfirmPopup popupPrefab;
        private Stack<ConfirmPopup> popupPool = new Stack<ConfirmPopup>();

        void OnEnable() => PopupEvents.OnConfirmationRequested += ShowPopup;
        void OnDisable() => PopupEvents.OnConfirmationRequested -= ShowPopup;

        private ConfirmPopup GetPopup()
        {
            if (popupPool.Count > 0) return popupPool.Pop();
            return Instantiate(popupPrefab, popupAnchor);
        }

        void ShowPopup(LocalizedString messageLocalized, Action onConfirm, Action onCancel)
        {
            ConfirmPopup popup = GetPopup();
            //uiManager.SetState(purchaseConfirmPopup);
            popup.gameObject.SetActive(true);
            popup.Setup(messageLocalized.GetLocalizedString(),
                () => onConfirm?.Invoke(),
                () => onCancel?.Invoke(),
                () => ReturnToPool(popup)
            );
        }

        private void ReturnToPool(ConfirmPopup popup)
        {
            popup.gameObject.SetActive(false);
            popupPool.Push(popup);
        }
    }
}
