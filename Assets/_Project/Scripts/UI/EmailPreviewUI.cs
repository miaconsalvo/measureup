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

        public void SetRead(bool read)
        {
            isRead = read;
            if (animator != null) animator.SetBool(readAnimParam, read);
        }
    }
}
