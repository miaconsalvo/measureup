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
            if (skipButton != null) skipButton.onClick.AddListener(SkipStage);
            if (completeStageButton != null)
                completeStageButton.gameObject.SetActive(false);
            dialogueRunner.StartDialogue(nodeStart);
        }

        protected void OnDialogueComplete()
        {
            dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueComplete);

            if (skipButton != null) skipButton.onClick.RemoveListener(SkipStage);
            if (completeStageButton != null)
                completeStageButton.gameObject.SetActive(true);
            else OnStageComplete();
        }

        protected void SkipStage()
        {
            if (!skipPopupText.IsEmpty)
                PopupEvents.RequestConfirmation(skipPopupText, OnStageComplete);
            else OnStageComplete();
        }

        protected void OnSkipStage()
        {
            OnStageComplete();
        }
    }
}
