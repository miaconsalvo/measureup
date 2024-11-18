using Mystie.Core;
using Mystie.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie
{
    public class PauseMenu : UIState
    {
        public override void DisplayState()
        {
            GameManager.Pause();
            base.DisplayState();
        }

        public override void CloseState()
        {
            base.CloseState();

            GameManager.Unpause();
        }

        public override void Pause()
        {
            Close();
        }
    }
}
