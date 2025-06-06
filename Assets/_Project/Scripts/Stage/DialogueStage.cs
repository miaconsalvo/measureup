using System.Collections;
using System.Collections.Generic;
using Mystie.Core;
using Mystie.Dialogue;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace Mystie
{
    public class DialogueStage : LevelStage
    {
        public DialogueRunner dialogueRunner;
        public string nodeStart;

        protected override void OnStageEnter()
        {
            base.OnStageEnter();

            dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
            if (completeStageButton != null)
                completeStageButton.gameObject.SetActive(false);
            dialogueRunner.StartDialogue(nodeStart);
        }

        protected void OnDialogueComplete()
        {
            dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueComplete);

            if (completeStageButton != null)
                completeStageButton.gameObject.SetActive(true);
            else OnStageComplete();
        }
    }
}
