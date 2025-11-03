using System;
using System.Collections;
using System.Collections.Generic;
using Mystie.Dressup;
using Mystie.UI;
using NaughtyAttributes;
using UnityEngine;

namespace Mystie.Dressup
{
    [CreateAssetMenu(fileName = "Generic Viewer Group", menuName = "Data/Viewer Group/Generic")]
    public class ViewerGroupScriptable : ScriptableObject
    {
        public string groupName;

        public Condition condition;
        public bool alwaysShowNeutralComments;
        public List<Comment> neutralComments;
        [ShowIf("HasCondition")] public List<Comment> posComments;
        [ShowIf("HasCondition")] public List<Comment> negComments;

        public virtual bool HasCondition => condition != null;

        public virtual List<Comment> GenerateComments(DressupManager dressup)
        {
            List<Comment> comments = new List<Comment>();

            switch (GetReaction(dressup))
            {
                case Reaction.Positive:
                    comments.AddRange(posComments);
                    break;
                case Reaction.Negative:
                    comments.AddRange(negComments);
                    break;
                case Reaction.Neutral:
                    if (!alwaysShowNeutralComments)
                        comments.AddRange(neutralComments);
                    break;
            }

            if (alwaysShowNeutralComments) comments.AddRange(neutralComments);

            return comments;
        }

        public virtual Reaction GetReaction(DressupManager dressup)
        {
            return HasCondition ? condition.Check(dressup) : Reaction.Neutral;
        }


    }
}
