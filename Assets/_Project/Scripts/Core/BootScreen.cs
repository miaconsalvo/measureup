using System.Collections;
using FMODUnity;
using Mystie.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.UI
{
    public class BootScreen : MonoBehaviour
    {
        [SerializeField] private GameObject loadingLabel;
        [SerializeField] private Button startButton;
        [SerializeField] private float bankTimeoutSeconds = 20f;

        private bool starting;

        private IEnumerator Start()
        {
            if (startButton != null)
            {
                startButton.gameObject.SetActive(false);
                startButton.onClick.AddListener(OnStartClicked);
            }
            if (loadingLabel != null) loadingLabel.SetActive(true);

            float elapsed = 0f;
            while ((!RuntimeManager.HaveAllBanksLoaded || RuntimeManager.AnySampleDataLoading())
                   && elapsed < bankTimeoutSeconds)
            {
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            if (elapsed >= bankTimeoutSeconds)
                Debug.LogWarning("BootScreen: FMOD banks did not finish loading in time; continuing anyway.");

            if (loadingLabel != null) loadingLabel.SetActive(false);
            if (startButton != null) startButton.gameObject.SetActive(true);
            else OnStartClicked();
        }

        private void OnDestroy()
        {
            if (startButton != null) startButton.onClick.RemoveListener(OnStartClicked);
        }

        private void OnStartClicked()
        {
            if (starting) return;
            starting = true;

            GameManager.Instance.LoadMainMenu();
        }
    }
}
