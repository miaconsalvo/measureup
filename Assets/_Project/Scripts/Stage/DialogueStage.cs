using System.Collections;
using System.Collections.Generic;
using Mystie.Core;
using Mystie.UI;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Yarn.Unity;

namespace Mystie
{
    public class DialogueStage : LevelStage
    {
        public string nodeStart;
        public bool skippable;
        [SerializeField] protected Button skipButton;
        [SerializeField] protected LocalizedString skipPopupText;

        protected override void OnStageEnter()
        {
            uiState.onExit += OnDialogueComplete;

            if (completeStageButton != null)
                completeStageButton.gameObject.SetActive(false);
            ((UIDialogueState)uiState).SetDialogue(nodeStart, skippable);

            base.OnStageEnter();

            Debug.Log("On stage enter");
        }

        protected override void OnStageComplete()
        {
            uiState.onExit -= OnDialogueComplete;

            base.OnStageComplete();
        }

        protected void OnDialogueComplete()
        {
            if (completeStageButton != null)
                completeStageButton.gameObject.SetActive(true);
            else OnStageComplete();
        }
    }
}
