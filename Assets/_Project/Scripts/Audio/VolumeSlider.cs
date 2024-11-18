using Mystie.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.Audio
{
    public class VolumeSlider : MonoBehaviour
    {
        private GameSettings gameSettings;
        public AudioBusType busType;
        public Slider slider;

        private void Awake()
        {
            gameSettings = GameManager.Instance.gameSettings;
        }

        private void OnEnable()
        {
            if (slider != null)
            {
                slider.normalizedValue = gameSettings.GetVolume(busType);
                slider.onValueChanged.AddListener(delegate { SetVolume(); });
            }
        }

        private void OnDisable()
        {
            if (slider != null)
            {
                slider.onValueChanged.RemoveListener(delegate { SetVolume(); });
            }
        }

        public void SetVolume()
        {
            float newVolume = slider.value / slider.maxValue;
            gameSettings.SetVolume(busType, newVolume);
        }
    }
}
