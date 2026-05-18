using FMODUnity;
using Mystie.UI;
using UnityEngine;
using UnityEngine.UI;

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
            animTimer.onTimerEnd += OnCatwalkDone;

            base.OnStageEnter();

            if (completeStageButton != null)
            {
                completeStageButton.gameObject.SetActive(false);
                //doneButton.onClick.AddListener(OnStageComplete);
            }
        }

        public void Update()
        {
            animTimer?.Tick(Time.deltaTime);
        }

        protected void OnCatwalkDone()
        {
            if (completeStageButton != null) completeStageButton.gameObject.SetActive(true);
            else OnStageComplete();
        }

        protected override void OnStageComplete()
        {
            //Destroy(dialogueModel);
            if (completeStageButton != null)
            {
                completeStageButton.gameObject.SetActive(false);
            }
            animTimer = null;
            base.OnStageComplete();
        }
    }
}
