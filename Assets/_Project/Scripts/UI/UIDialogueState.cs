using System.Collections;
using Mystie.Dialogue;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Yarn.Unity;

namespace Mystie.UI
{
    public class UIDialogueState : UIState
    {
        public DialogueRunner dialogueRunner;
        public string startNode;
        public bool skippable;
        [SerializeField] protected Button skipButton;
        [SerializeField] protected LocalizedString skipPopupText;

        public override IEnumerator DisplayState()
        {
            yield return StartCoroutine(base.DisplayState());

            Debug.Log("Started convo: " + startNode);

            if (skipButton != null)
            {
                skipButton.gameObject.SetActive(skippable);
                skipButton.onClick.AddListener(SkipDialogue);
            }

            dialogueRunner.onDialogueComplete.AddListener(CloseState);
            dialogueRunner.StartDialogue(startNode);
        }

        public override IEnumerator HideState(bool immediate = false)
        {
            Debug.Log("=== HideState START ===");
            Debug.Log("Current location before reset: "
            + (DialogueManager.Instance.currentLocation != null ? DialogueManager.Instance.currentLocation.name : "null"));

            startNode = string.Empty;

            dialogueRunner.onDialogueComplete.RemoveListener(CloseState);

            if (dialogueRunner.IsDialogueRunning)
            {
                Debug.Log("Stopping dialogue runner");
                dialogueRunner.Stop();
            }

            Debug.Log("Calling DialogueManagerCommands.ResetScene()");
            DialogueManagerCommands.ResetScene();

            if (skipButton != null)
            {
                skipButton.gameObject.SetActive(false);
                skipButton.onClick.RemoveListener(SkipDialogue);
            }

            yield return StartCoroutine(base.HideState(immediate));

            Debug.Log("=== HideState END ===");

            /*if (changeScene)
            {
                if (transition != null) SceneTransitioner.Instance.LoadScene(nextScene, transition);
                else SceneTransitioner.Instance.LoadScene(nextScene);
            }*/
        }

        public void SetDialogue(string startNode, bool skippable = true)
        {
            this.startNode = startNode;
            this.skippable = skippable;
        }

        public void SetState(string startNode, bool skippable = true)
        {
            if (startNode == string.Empty) return;
            SetDialogue(startNode, skippable);
            SetState();
        }

        protected void SkipDialogue()
        {
            if (!skipPopupText.IsEmpty)
                PopupEvents.RequestConfirmation(skipPopupText, CloseState);
            else CloseState();
        }
    }
}
