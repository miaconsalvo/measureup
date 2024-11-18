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

        public void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}
