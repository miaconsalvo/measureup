using System.Collections;
using System.Collections.Generic;
using Mystie.UI;
using UnityEngine;

namespace Mystie.Core
{
    public class PostEpisodeStage : LevelStage
    {
        public SocialMediaUI socialMediaUI;

        protected override void OnStageEnter()
        {
            CommentCollection comments = LevelManager.Instance.episode.socialMediaComments;
            socialMediaUI.QueueComments(comments.GenerateComments(LevelManager.Instance.dressup));
            base.OnStageEnter();
        }
    }
}
