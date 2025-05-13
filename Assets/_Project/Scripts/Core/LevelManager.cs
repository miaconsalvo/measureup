using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie.Core
{
    public class LevelManager : MonoBehaviour
    {
        #region Singleton

        private static LevelManager instance;
        public static LevelManager Instance {
            get {
                if (instance == null)
                    instance = FindObjectOfType<LevelManager>();

                return instance;
            }
        }

        #endregion

        #region Events

        public event Action<LevelStageType> onStageSet;
        
        #endregion

        private int stageIndex;
        public List<LevelStageType> stages = new List<LevelStageType>();
        public LevelStageType CurrentStage {
            get => stageIndex < stages.Count? stages[stageIndex] : stages[stages.Count-1];
        }

        public void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            Debug.Log("Level manager start");

            stageIndex = 0;
            onStageSet?.Invoke(CurrentStage);
        }

        public void CompleteStage(LevelStageType stage)
        {
            if (CurrentStage == stage)
            {
                stageIndex++;

                if (stageIndex >= stages.Count) {
                    Debug.Log("Level Complete!");
                }
                else
                {
                    Debug.Log("Stage set " + CurrentStage);
                    onStageSet?.Invoke(CurrentStage);
                }
            }
        }
    }
}
