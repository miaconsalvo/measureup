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
        public StyleRuleScriptable rule;
        public CommentCollection socialMediaComments;
        public int maxQuestions = 3;
        public List<InterviewQuestion> interviewNotes;
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
