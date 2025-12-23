using UnityEngine;
using Yarn.Unity;

namespace Mystie.UI
{
    public class CutsceneState : UIState
    {
        public DialogueRunner dialogueRunner;
        public EpisodeManager episodeManager { get; private set; }
        public string nodeStart;

        protected override void Awake()
        {
            base.Awake();
            episodeManager = EpisodeManager.Instance;
        }

        public override void CloseState()
        {
            base.CloseState();
            Debug.Log("Cutscene Complete!");
        }

        public void OnLevelComplete()
        {
            SaveDataManager.SaveEpisodeIndex(episodeManager.index + 1);
            SaveDataManager.SaveGameData();

            episodeManager.CompleteEpisode();
            episodeManager.LoadCurrentEpisode();
        }
    }
}
