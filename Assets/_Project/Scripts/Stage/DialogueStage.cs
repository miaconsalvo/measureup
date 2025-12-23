using System.Collections;
using System.Collections.Generic;
using Mystie.Core;
using Mystie.Dialogue;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Yarn.Unity;

namespace Mystie
{
    public class DialogueStage : LevelStage
    {
        public DialogueRunner dialogueRunner;
        public string nodeStart;
        [SerializeField] protected Button skipButton;
        [SerializeField] protected LocalizedString skipPopupText;

        protected override void OnStageEnter()
        {
            base.OnStageEnter();

            dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
            if (skipButton != null)
            {
                skipButton.gameObject.SetActive(true);
                skipButton.onClick.AddListener(OnSkipDialogue);
            }
            if (completeStageButton != null)
                completeStageButton.gameObject.SetActive(false);
            dialogueRunner.StartDialogue(nodeStart);

            Debug.Log("On stage enter");
        }

        protected override void OnStageComplete()
        {
            DialogueManagerCommands.ResetScene();
            dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueComplete);

            if (skipButton != null)
            {
                skipButton.gameObject.SetActive(false);
                skipButton.onClick.RemoveListener(OnSkipDialogue);
            }

            base.OnStageComplete();
        }

        protected void OnDialogueComplete()
        {
            if (completeStageButton != null)
                completeStageButton.gameObject.SetActive(true);
            else OnStageComplete();
        }

        protected void SkipDialogue()
        {
            if (!skipPopupText.IsEmpty)
                PopupEvents.RequestConfirmation(skipPopupText, OnStageComplete);
            else OnStageComplete();
        }

        protected void OnSkipDialogue()
        {
            dialogueRunner.Stop();
        }
    }
}
