using Mystie.Core;
using Mystie.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie
{
    public class UIStatePause : UIState
    {
        public override void DisplayState()
        {
            GameManager.Pause();
            base.DisplayState();
        }

        public override void CloseState()
        {
            GameManager.Unpause();
            base.CloseState();
        }

        public override void Pause()
        {
            Close();
        }
    }
}
