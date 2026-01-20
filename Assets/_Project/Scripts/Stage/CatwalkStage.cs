using Mystie.UI;
using UnityEngine;

namespace Mystie.Core
{
    public class CatwalkStage : LevelStage
    {
        private SpriteLayered dialogueModel;

        protected override void OnStageEnter()
        {
            dialogueModel = Instantiate(DressupUIManager.Instance.dressupUI.modelUI.model);

            base.OnStageEnter();
        }

        protected override void OnStageComplete()
        {
            Destroy(dialogueModel);

            base.OnStageComplete();
        }
    }
}
