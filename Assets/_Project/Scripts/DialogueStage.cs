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
        public string nodeEnd;


        protected override void OnStageEnter()
        {
            base.OnStageEnter();

            if (completeStageButton != null)
                completeStageButton.gameObject.SetActive(false);
            dialogueRunner.StartDialogue(nodeStart);
        }

        protected override void OnStageSet(LevelStageType newStage)
        {
            base.OnStageSet(newStage);
            dialogueRunner.onNodeComplete.AddListener(OnNodeComplete);
        }

        protected void OnNodeComplete(string node)
        {
            if (node == nodeEnd)
            {
                if (completeStageButton != null)
                    completeStageButton.gameObject.SetActive(true);
                else OnStageComplete();
            }
        }

        protected override void OnStageComplete()
        {
            dialogueRunner.onNodeComplete.RemoveListener(OnNodeComplete);
            base.OnStageComplete();
        }
    }
}
