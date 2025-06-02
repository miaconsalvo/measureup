using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Mystie.UI
{
    public class ConfirmPopup : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private LocalizeStringEvent messageTextLocalized;
        [SerializeField] private TMPro.TextMeshProUGUI messageText;

        [Space]

        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        [Space]

        [SerializeField] private string openAnim = "Open";
        [SerializeField] private string closeAnim = "Close";

        private Action onConfirm;
        private Action onCancel;
        private Action onClose;

        private void OnEnable()
        {
            if (confirmButton != null) confirmButton.onClick.AddListener(Confirm);
            if (cancelButton != null) cancelButton.onClick.AddListener(Cancel);
        }

        private void OnDisable()
        {
            if (confirmButton != null) confirmButton.onClick.RemoveListener(Confirm);
            if (cancelButton != null) cancelButton.onClick.RemoveListener(Cancel);
        }

        public void Setup(LocalizedString messageLocalized, Action confirmAction, Action cancelAction, Action closeAction)
        {
            messageTextLocalized.StringReference = messageLocalized;
            Setup(messageLocalized.GetLocalizedString(), confirmAction, cancelAction, closeAction);
        }

        public void Setup(string message, Action confirmAction, Action cancelAction, Action closeAction)
        {
            messageText.text = message;
            onConfirm = confirmAction;
            onCancel = cancelAction;
            onClose = closeAction;

            //gameObject.SetActive(true);
            animator.Play(openAnim);
        }

        public void Confirm() => StartCoroutine(CloseRoutine(true));
        public void Cancel() => StartCoroutine(CloseRoutine(false));

        private IEnumerator CloseRoutine(bool confirmed)
        {
            animator.Play(closeAnim);

            if (confirmed) onConfirm?.Invoke();
            else onCancel?.Invoke();

            onClose?.Invoke();

            yield return new WaitForSeconds(0.5f); // Match animation length

            onConfirm = null;
            onCancel = null;
            onClose = null;

            //gameObject.SetActive(false);
        }

        private void Reset()
        {
            animator = GetComponent<Animator>();
            messageTextLocalized = GetComponentInChildren<LocalizeStringEvent>();
            messageText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }
    }
}
