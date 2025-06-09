/*
Yarn Spinner is licensed to you under the terms found in the file LICENSE.md.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using Yarn.Unity;

namespace Mystie.UI
{
    /// <summary>
    /// clones dialogue bubbles for the ChatDialogue example
    /// </summary>
    public class PhoneChatDialogueHelper : DialogueViewBase
    {
        DialogueRunner runner;

        [SerializeField] private RectTransform messageContainer;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private ChatBubbleUI messageBoxPrefab;
        [SerializeField] private RectTransform optionsContainer;
        [SerializeField] private OptionView optionPrefab;
        [SerializeField] private float lettersPerSecond = 10f;
        [SerializeField] private float delayPerCharacter = .05f;

        [Space]

        [SerializeField] private MessageBoxSettings messageBoxMe;
        [SerializeField] private MessageBoxSettings messageBoxThem;
        private MessageBoxSettings messageBoxNext;

        private ChatBubbleUI messageBox;

        void Awake()
        {
            runner = GetComponent<DialogueRunner>();
            runner.AddCommandHandler("Me", SetSenderMe); // registers Yarn Command <<Me>>, which sets the current message sender to "Me"
            runner.AddCommandHandler("Them", SetSenderThem); // registers Yarn Command <<They>>, which sets the current message sender to "Them" (whoever the player is talking to)

            optionsContainer.gameObject.SetActive(false);
        }

        public void LateUpdate()
        {
            if (currentTypewriterEffect != null)
            {
                scrollRect.verticalNormalizedPosition = 0f;
                LayoutRebuilder.ForceRebuildLayoutImmediate(messageContainer);
            }
        }

        // YarnCommand <<Me>>, but does not use YarnCommand C# attribute, registers in Awake() instead
        public void SetSenderMe()
        {
            messageBoxNext = messageBoxMe;
        }

        // YarnCommand <<Them>> does not use YarnCommand C# attribute, registers in Awake() instead
        public void SetSenderThem()
        {
            messageBoxNext = messageBoxThem;
        }

        public void InstantiateMessageBox()
        {
            messageBox = Instantiate(messageBoxPrefab.gameObject, messageContainer).GetComponent<ChatBubbleUI>();
            messageBox.transform.SetAsLastSibling();
            messageBox.Set(messageBoxNext);
        }

        Coroutine currentTypewriterEffect;

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            if (currentTypewriterEffect != null)
            {
                StopCoroutine(currentTypewriterEffect);
            }



            InstantiateMessageBox();
            messageBox.SetText(dialogueLine.TextWithoutCharacterName.Text, dialogueLine.CharacterName);

            float delay = delayPerCharacter * dialogueLine.TextWithoutCharacterName.Text.Length;
            currentTypewriterEffect = StartCoroutine(ShowTextAndNotify());

            IEnumerator ShowTextAndNotify()
            {
                messageBox.SetLoading(true);

                yield return new WaitForSeconds(delay);

                messageBox.SetLoading(false);

                yield return StartCoroutine(Effects.Typewriter(messageBox.text, lettersPerSecond, null));

                currentTypewriterEffect = null;
                onDialogueLineFinished();
            }
        }

        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            foreach (Transform child in optionsContainer.transform)
            {
                Destroy(child.gameObject);
            }

            optionsContainer.gameObject.SetActive(true);

            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                DialogueOption option = dialogueOptions[i];
                var optionView = Instantiate(optionPrefab);

                optionView.transform.SetParent(optionsContainer, false);
                LayoutRebuilder.ForceRebuildLayoutImmediate(optionsContainer);

                optionView.Option = option;

                optionView.OnOptionSelected = (selectedOption) =>
                {
                    optionsContainer.gameObject.SetActive(false);
                    onOptionSelected(selectedOption.DialogueOptionID);
                };
            }
        }
    }
}
