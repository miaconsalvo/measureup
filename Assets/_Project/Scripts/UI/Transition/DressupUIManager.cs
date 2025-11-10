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
        [field: SerializeField] public DressupStage dressupUI { get; private set; }
        [field: SerializeField] public DialogueStage interviewUI { get; private set; }
        [field: SerializeField] public DialogueStage postCatwalkUI { get; private set; }
        [field: SerializeField] public TabletStage preChatUI { get; private set; }
        [field: SerializeField] public TabletStage postChatUI { get; private set; }

        public void Initialize(LevelManager levelManager)
        {
            this.levelManager = levelManager;
            EpisodeScriptable episode = levelManager.episode;

            dossierUI.Initialize(episode);
            storeUI.Initialize(episode);
            dressupUI.Initialize(episode);

            interviewUI.nodeStart = episode.interviewNode;
            postCatwalkUI.nodeStart = episode.postCatwalkNode;

            preChatUI.nodeStart = episode.preChatNode;
            postChatUI.nodeStart = episode.postChatNode;
        }

        public void Cleanup()
        {

        }
    }
}
