using System.Collections;
using System.Collections.Generic;
using Mystie.UI;
using UnityEngine;

namespace Mystie.Core
{
    public class PostEpisodeStage : TabletStage
    {
        public SocialMediaUI socialMediaUI;

        protected override void InitializeTablet()
        {
            base.InitializeTablet();

            CommentCollection comments = LevelManager.Instance.episode.socialMediaComments;
            socialMediaUI.QueueComments(comments.GenerateComments(LevelManager.Instance.dressup));
        }
    }
}
