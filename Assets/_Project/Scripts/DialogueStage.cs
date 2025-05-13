using System.Collections;
using System.Collections.Generic;
using Mystie.Core;
using Mystie.Dialogue;
using UnityEngine;
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

            dialogueRunner.StartDialogue(nodeStart);
        }

        protected override void OnStageSet(LevelStageType newStage)
        {
            base.OnStageSet(newStage);
            dialogueRunner.onNodeComplete.AddListener(OnNodeComplete);
        }

        protected void OnNodeComplete(string node)
        {
            if (node == nodeEnd) OnStageComplete();
        }

        protected override void OnStageComplete()
        {
            dialogueRunner.onNodeComplete.RemoveListener(OnNodeComplete);
            base.OnStageComplete();
        }
    }
}
