using Mystie.Core;
using Mystie.Dressup;
using UnityEngine;

namespace Mystie.UI
{
    public class DressupUIManager : UIManager
    {
        public LevelManager levelManager { get; private set; }

        #region Singleton

        public new static DressupUIManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<DressupUIManager>();
                return instance;
            }
        }

        protected new static DressupUIManager instance;

        #endregion

        [field: SerializeField] public DossierUI dossierUI { get; private set; }
        [field: SerializeField] public StoreStage storeUI { get; private set; }

        public void Initialize(LevelManager levelManager)
        {
            this.levelManager = levelManager;

            dossierUI.Initialize(levelManager.episode);
            storeUI.Initialize(levelManager.episode);
        }

        public void Cleanup()
        {

        }
    }
}
