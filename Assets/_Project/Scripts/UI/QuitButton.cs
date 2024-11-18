using Mystie.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.UI
{
    public class QuitButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        private void Start()
        {
            if (button == null) 
                button = GetComponentInChildren<Button>();
            button.onClick.AddListener(GameManager.Quit);
        }

        private void OnReset()
        {
            button = GetComponentInChildren<Button>();
        }
    }
}
