using Mystie.Core;
using Mystie.UI.Transition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mystie.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private SceneTransitionMode transitionMode;
        [SerializeField]
        private Button playBtn;

        [Space]

        [SerializeField]
        private string startScene;

        private void OnEnable()
        {
            if (playBtn != null) playBtn.onClick.AddListener(OnPlay);
        }

        private void OnDisable()
        {
            if (playBtn != null) playBtn.onClick.RemoveListener(OnPlay);
        }

        private void OnPlay()
        {
            SceneTransitioner.Instance.LoadScene(startScene, transitionMode);
        }
    }
}
