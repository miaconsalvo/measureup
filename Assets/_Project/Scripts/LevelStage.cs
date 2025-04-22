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

        public LevelManager levelManager{get; protected set;}
        public UIManager uiManager{get; protected set;}

        public bool active { get; private set; }

        [SerializeField] public UIState uiState;

        [field: SerializeField] public LevelStageType stage{get; protected set;}

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

        public void OnStageSet(LevelStageType newStage)
        {
            if (newStage == stage && !active)
            {
                OnStageEnter();
            }
        }

        public virtual void OnStageEnter() {
            active = true;
            uiManager.SetState(uiState);
            onStageEnter?.Invoke();
            Debug.Log("On stage enter " + stage);
        }

        public virtual void OnStageComplete() {
            active = false;
            uiManager.CloseState();
            onStageComplete?.Invoke();
            Debug.Log("Stage complete!");
            levelManager.CompleteStage(stage);
        }
    }

}
