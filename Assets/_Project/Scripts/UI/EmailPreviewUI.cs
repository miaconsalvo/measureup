using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.UI
{
    public class EmailPreviewUI : MessageBoxUI
    {
        private Animator animator;

        public Button button { get; private set; }
        public Email email;
        public string readAnimParam = "read";
        private bool isRead;

        private void Awake()
        {
            button = GetComponent<Button>();
            animator = GetComponent<Animator>();
            //SetRead(false);
        }

        private void OnDestroy()
        {
            if (button != null) button.onClick.RemoveAllListeners();
        }

        public void Set(Email email)
        {
            this.email = email;
            SetText(email.subject, email.sender);
            SetRead(email.read);
        }

        public void SetRead(bool read)
        {
            isRead = read;
            email.read = isRead;
            if (animator != null) animator.SetBool(readAnimParam, read);
        }
    }
}
