using System.Collections;
using System.Collections.Generic;
using Mystie.UI;
using UnityEngine;

namespace Mystie.Core
{
    public class LevelStage : MonoBehaviour
    {
        private LevelManager levelManager;

        [SerializeField] public UIState startState;
        [SerializeField] public LevelStage nextStage;
        
        void Awake()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void NextState(){

        }
    }

}
