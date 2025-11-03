using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace Mystie
{
    public class ChatUI : AppUI
    {
        public DialogueRunner dialogueRunner;

        public Transform anchor;

        private string convoNodeStart;

        [SerializeField] protected Button completeStageButton;

        public override void OnOpen()
        {
            if (open == true) return;
            base.OnOpen();
            if (convoNodeStart == string.Empty) return;

            if (completeStageButton != null)
                completeStageButton.gameObject.SetActive(false);
            if (convoNodeStart != string.Empty) StartConvo(convoNodeStart);
        }

        public void QueueConvo(string nodeStart)
        {
            convoNodeStart = nodeStart;
        }

        public void StartConvo(string nodeStart)
        {
            dialogueRunner.onDialogueComplete.AddListener(OnConvoEnd);
            dialogueRunner.StartDialogue(nodeStart);
        }

        private void OnConvoEnd()
        {
            if (completeStageButton != null)
                completeStageButton.gameObject.SetActive(true);
            if (appNavbarUI != null && lockNavbar) appNavbarUI.SetNavbarEnabled(true);
        }

        public override void Clear()
        {
            base.Clear();
            /*while (anchor.childCount > 0)
            {
                Destroy(anchor.GetChild(anchor.childCount - 1).gameObject);
            }*/
        }
    }
}
