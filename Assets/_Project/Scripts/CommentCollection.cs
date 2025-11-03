using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mystie.Dressup;
using NaughtyAttributes;
using UnityEngine;

namespace Mystie.UI
{
    [CreateAssetMenu(fileName = "Comment Collection", menuName = "Data/Comment Collection", order = 0)]
    public class CommentCollection : ScriptableObject
    {
        [Expandable] public List<ViewerGroupScriptable> viewerGroups;

        public List<Comment> GenerateComments(DressupManager dressup)
        {
            List<Comment> comments = new List<Comment>();

            foreach (ViewerGroupScriptable group in viewerGroups)
            {
                comments.AddRange(group.GenerateComments(dressup));
            }

            comments = comments.OrderBy(x => UnityEngine.Random.value).ToList();
            return comments;
        }
    }
}
