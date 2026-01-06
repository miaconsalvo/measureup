using Mystie.Core;
using Mystie.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie
{
    public class UIStatePause : UIState
    {
        public override IEnumerator DisplayState()
        {
            GameManager.Pause();
            yield return StartCoroutine(base.DisplayState());
        }

        public override void CloseState()
        {
            GameManager.Unpause();
            base.CloseState();
        }

        public override void Pause()
        {
            CloseState();
        }
    }
}
