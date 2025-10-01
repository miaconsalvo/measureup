using System;
using System.Collections;
using System.Collections.Generic;
using Mystie.Dressup;
using Mystie.UI;
using UnityEngine;
using UnityEngine.Localization;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Episode", menuName = "Data/Episode", order = 0)]
    public class EpisodeScriptable : ScriptableObject
    {
        public ContestantData contestant;
        public StyleRuleScriptable styleRule;
        public StyleRuleScriptable trendingRule;
        public CommentCollection socialMediaComments;

        [Header("Narrative")]

        public string preChatNode;
        public string interviewNode;
        public string postCatwalkNode;
        public string postChatNode;

        [Header("Dossier")]

        public LocalizedString logline;
        public LocalizedString trendingDescription;

        [Header("Interview")]
        public int maxQuestions = 3;
        public List<InterviewQuestion> interviewNotes;

        [field: SerializeField] public List<ClothingKitScriptable> kitsOnSale { get; private set; }
    }

    [Serializable]
    public class InterviewQuestion
    {
        [TextArea] public string question;
        public List<InterviewNote> notes;
    }

    [Serializable]
    public class InterviewNote
    {
        public string name;
        public enum InfoType { Like, Dislike, Lifestyle }
        public InfoType type;
        public LocalizedString text;
    }
}
