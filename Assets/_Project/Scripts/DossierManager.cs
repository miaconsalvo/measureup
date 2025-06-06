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
    public class DossierManager : MonoBehaviour
    {
        public static LevelManager levelManager;

        [field: SerializeField] public NavButton dossierUI { get; private set; }
        [field: SerializeField] public RectTransform dossierRect { get; private set; }
        [SerializeField] private List<LevelStageType> stagesWithDossier;
        public EpisodeScriptable episode;
        private ContestantData contestant;

        public LocalizeStringEvent loglineText;
        public LocalizeStringEvent styleRuleText;
        public LocalizeStringEvent styleRuleDescriptionText;
        public LocalizeStringEvent trendingText;
        public TextMeshProUGUI contestantNameText;
        public LocalizeStringEvent occupationText;

        private List<int> questionsAsked;

        [Space]

        public TextMeshProUGUI likes;
        public TextMeshProUGUI dislikes;
        public Transform lifestyleAnchor;
        public LocalizeStringEvent lifestyleNotePrefab;

        private List<LocalizedString> likesList;
        private List<LocalizedString> dislikesList;

        public void Awake()
        {
            levelManager = LevelManager.Instance;
            levelManager.onStageSet += OnStageSet;
            dossierUI.Sub(UIManager.Instance);
            dossierUI.state.onDisplay += UpdateUI;
        }

        public void OnDestroy()
        {
            levelManager.onStageSet -= OnStageSet;
            dossierUI.Unsub();
            dossierUI.state.onDisplay -= UpdateUI;
        }

        public void SetEpisode(EpisodeScriptable episode)
        {
            this.episode = episode;
            contestant = episode.contestant;

            loglineText.StringReference = episode.logline;
            styleRuleText.StringReference = episode.rule.ruleName;
            styleRuleDescriptionText.StringReference = episode.rule.ruleDescription;
            trendingText.StringReference = episode.trending;
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

        [YarnCommand("add_note")]
        public static void AddNote(int q, int n)
        {
            DossierManager dossier = levelManager.dossier;
            InterviewQuestion question = levelManager.episode.interviewNotes[n];
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
                            Instantiate(levelManager.dossier.lifestyleNotePrefab.gameObject
                                , levelManager.dossier.lifestyleAnchor)
                                .GetComponent<LocalizeStringEvent>();
                            noteString.StringReference = note.text;
                            break;
                    }
                }

                levelManager.dossier.questionsAsked.Add(q);
                levelManager.dossier.UpdateUI();
            }
        }

        [YarnFunction("question_asked")]
        public static bool IsQuestionAsked(int i)
        {
            return levelManager.dossier.questionsAsked.Contains(i);
        }

        [YarnFunction("max_questions")]
        public static bool MaxQuestions()
        {
            return levelManager.dossier.questionsAsked.Count
                >= levelManager.episode.maxQuestions;
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
    }
}
