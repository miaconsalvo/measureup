using FMODUnity;
using Mystie.UI;
using UnityEngine;

namespace Mystie.Core
{
    public class CatwalkStage : LevelStage
    {

        public Animator animator;
        public SpriteLayered dialogueModel;
        public string playAnimParam = "Play";
        public float animDuration = 24f;

        private Timer animTimer;

        protected override void OnStageEnter()
        {
            dialogueModel.Set(DressupUIManager.Instance.dressupUI.modelUI.model);
            animator.SetTrigger(playAnimParam);

            animTimer = new Timer(animDuration);
            animTimer.onTimerEnd += CompleteStage;

            base.OnStageEnter();
        }

        public void Update()
        {
            animTimer?.Tick(Time.deltaTime);
        }

        protected override void OnStageComplete()
        {
            //Destroy(dialogueModel);

            base.OnStageComplete();
        }
    }
}
