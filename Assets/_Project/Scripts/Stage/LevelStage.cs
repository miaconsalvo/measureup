using System;
using System.Collections;
using System.Collections.Generic;
using Mystie.UI;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Mystie.Core
{
    public class LevelStage : MonoBehaviour
    {
        public event Action onStageEnter;
        public event Action onStageComplete;

        public LevelManager levelManager { get; protected set; }
        public UIManager uiManager { get; protected set; }

        public bool active { get; private set; }

        [SerializeField] public UIState uiState;

        [field: SerializeField] public LevelStageType stage { get; protected set; }

        [Space]

        [SerializeField] private Button completeStageButton;
        [SerializeField] private LocalizedString stageCompletePopupText;

        protected virtual void Awake()
        {
            levelManager = LevelManager.Instance;
            uiManager = UIManager.Instance;
        }

        protected virtual void OnEnable()
        {
            levelManager.onStageSet += OnStageSet;
            if (completeStageButton != null) completeStageButton.onClick.AddListener(CompleteStage);
            //uiState.onSubmit += CompleteStage;
        }

        protected virtual void OnDisable()
        {
            levelManager.onStageSet -= OnStageSet;
            if (completeStageButton != null) completeStageButton.onClick.RemoveListener(CompleteStage);
            //uiState.onSubmit -= CompleteStage;
        }

        protected virtual void OnDestroy()
        {

        }

        protected virtual void OnStageSet(LevelStageType newStage)
        {
            if (newStage == stage && !active)
            {
                OnStageEnter();
            }
        }

        protected virtual void OnStageEnter()
        {
            active = true;
            uiManager.SetState(uiState);
            onStageEnter?.Invoke();
            Debug.Log("On stage enter " + stage);
        }

        protected virtual void CompleteStage()
        {
            if (!stageCompletePopupText.IsEmpty)
                PopupEvents.RequestConfirmation(stageCompletePopupText, OnStageComplete);
            else OnStageComplete();
        }

        protected virtual void OnStageComplete()
        {
            Debug.Log("Stage complete!");
            active = false;
            uiManager.CloseState();
            onStageComplete?.Invoke();
            levelManager.CompleteStage(stage);
        }
    }

}
