using System;
using System.Collections;
using System.Collections.Generic;
using Mystie.UI;
using UnityEngine;

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

        protected virtual void Awake()
        {
            levelManager = LevelManager.Instance;
            uiManager = UIManager.Instance;

            levelManager.onStageSet += OnStageSet;
            uiState.onSubmit += OnStageComplete;
        }

        protected virtual void OnDestroy()
        {
            levelManager = LevelManager.Instance;
            levelManager.onStageSet -= OnStageSet;
            uiState.onSubmit -= OnStageComplete;
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
