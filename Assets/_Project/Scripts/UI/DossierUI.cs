using System;
using System.Collections;
using System.Collections.Generic;
using Mystie.Core;
using Mystie.Dressup;
using Mystie.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Yarn.Unity;

namespace Mystie
{
    public class DossierUI : MonoBehaviour
    {
        [field: SerializeField] public NavButton dossierUI { get; private set; }
        [field: SerializeField] public RectTransform dossierRect { get; private set; }
        [SerializeField] private List<LevelStageType> stagesWithDossier;
        private EpisodeScriptable episode;
        private ContestantData contestant;

        public LocalizeStringEvent loglineText;
        public LocalizeStringEvent styleRuleText;
        public LocalizeStringEvent styleRuleDescriptionText;
        public TextMeshProUGUI contestantNameText;
        public LocalizeStringEvent occupationText;
        public LocalizeStringEvent trendingText;

        private List<int> questionsAsked;

        [Space]

        public TextMeshProUGUI likes;
        public TextMeshProUGUI dislikes;
        public Transform lifestyleAnchor;
        public LocalizeStringEvent lifestyleNotePrefab;

        private List<LocalizedString> likesList;
        private List<LocalizedString> dislikesList;

        public void OnEnable()
        {
            dossierUI.Sub(UIManager.Instance);
            dossierUI.state.onDisplay += UpdateUI;

            LevelManager.Instance.onStageSet += OnStageSet;
        }

        public void OnDisable()
        {
            dossierUI.Unsub();
            dossierUI.state.onDisplay -= UpdateUI;

            LevelManager.Instance.onStageSet -= OnStageSet;
        }

        public void Initialize(EpisodeScriptable episode)
        {
            this.episode = episode;
            contestant = episode.contestant;

            loglineText.StringReference = episode.logline;
            styleRuleText.StringReference = episode.styleRule.ruleName;
            trendingText.StringReference = episode.trendingDescription;
            styleRuleDescriptionText.StringReference = episode.styleRule.ruleDescription;
            contestantNameText.text = contestant.name + ", " + contestant.age;
            occupationText.StringReference = contestant.occupation;

            questionsAsked = new List<int>();
            likesList = new List<LocalizedString>();
            dislikesList = new List<LocalizedString>();
            likes.text = "";
            dislikes.text = "";
            for (int i = lifestyleAnchor.childCount - 1; i >= lifestyleAnchor.childCount; i--)
                Destroy(lifestyleAnchor.GetChild(i).gameObject);
        }

        public void UpdateUI()
        {
            if (dossierRect != null) LayoutRebuilder.ForceRebuildLayoutImmediate(dossierRect);
        }

        public static string GetList(List<LocalizedString> stringList)
        {
            string str = "";
            for (int i = 0; i < stringList.Count; i++)
            {
                if (!stringList[i].IsEmpty)
                {
                    if (i > 0) str += ", ";
                    str += stringList[i].GetLocalizedString();
                }
            }
            return str;
        }

        private void OnStageSet(LevelStageType type)
        {
            dossierUI.btn.gameObject.SetActive(stagesWithDossier.Contains(type));
        }

        [YarnCommand("add_note")]
        public static void AddNote(int q, int n)
        {
            DossierUI dossier = DressupUIManager.Instance.dossierUI;
            InterviewQuestion question = LevelManager.Instance.episode.interviewNotes[n];
            if (!dossier.questionsAsked.Contains(q))
            {
                foreach (InterviewNote note in question.notes)
                {
                    if (note == null || note.text.IsEmpty) continue;

                    switch (note.type)
                    {
                        case InterviewNote.InfoType.Like:
                            if (!dossier.likesList.Contains(note.text))
                            {
                                dossier.likesList.Add(note.text);
                                dossier.likes.text = GetList(dossier.likesList);
                            }
                            break;
                        case InterviewNote.InfoType.Dislike:
                            if (!dossier.dislikesList.Contains(note.text))
                            {
                                dossier.dislikesList.Add(note.text);
                                dossier.dislikes.text = GetList(dossier.dislikesList);
                            }
                            break;
                        case InterviewNote.InfoType.Lifestyle:
                            LocalizeStringEvent noteString =
                            Instantiate(dossier.lifestyleNotePrefab.gameObject
                                , dossier.lifestyleAnchor)
                                .GetComponent<LocalizeStringEvent>();
                            noteString.StringReference = note.text;
                            break;
                    }
                }

                dossier.questionsAsked.Add(q);
                dossier.UpdateUI();
            }
        }

        [YarnFunction("question_asked")]
        public static bool IsQuestionAsked(int i)
        {
            return DressupUIManager.Instance.dossierUI.questionsAsked.Contains(i);
        }

        [YarnFunction("max_questions")]
        public static bool MaxQuestions()
        {
            return DressupUIManager.Instance.dossierUI.questionsAsked.Count
                >= DressupUIManager.Instance.dossierUI.episode.maxQuestions;
        }
    }
}
