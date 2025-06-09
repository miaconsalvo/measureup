using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Mystie.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.UI
{
    public class SocialMediaUI : AppUI
    {
        [SerializeField] private RectTransform messageContainer;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private ChatBubbleUI messageBoxPrefab;
        [SerializeField] private MessageBoxSettings messageBoxSettings;
        [SerializeField] private float delayBetweenPostsMin = 1f;
        [SerializeField] private float delayBetweenPostsMax = 2f;
        [SerializeField] private EventReference postSFX;

        private ChatBubbleUI messageBox;
        private Queue<Comment> commentsQueue;
        private bool displayingComments;

        public override void OnOpen()
        {
            if (open == true) return;
            open = true;

            if (lockNavbar && appNavbarUI != null && !commentsQueue.IsNullOrEmpty())
                appNavbarUI.SetNavbarEnabled(false);

            if (!commentsQueue.IsNullOrEmpty() && !displayingComments)
                StartCoroutine(DisplayComments());
        }

        public override void OnClose()
        {
            if (open == false) return;
            base.OnClose();
            //Debug.Log("Social Media close!");
            StopCoroutine(DisplayComments());
            displayingComments = false;
        }

        public void LateUpdate()
        {
            if (displayingComments)
            {
                scrollRect.verticalNormalizedPosition = 0f;
                LayoutRebuilder.ForceRebuildLayoutImmediate(messageContainer);
            }
        }

        public void QueueComments(List<Comment> comments)
        {
            if (comments.IsNullOrEmpty()) return;

            if (commentsQueue == null) commentsQueue = new Queue<Comment>();
            //Debug.Log(comments.Count + " comments queued.");
            foreach (Comment comment in comments)
                commentsQueue.Enqueue(comment);
        }

        public IEnumerator DisplayComments()
        {
            displayingComments = true;

            //Debug.Log("Displaying comments");

            while (commentsQueue.Count > 0)
            {
                DisplayComment(commentsQueue.Dequeue());

                //scrollRect.verticalNormalizedPosition = 0f;
                //LayoutRebuilder.ForceRebuildLayoutImmediate(messageContainer);

                float delay = Random.Range(delayBetweenPostsMin, delayBetweenPostsMax);
                yield return new WaitForSeconds(delay);
            }

            displayingComments = false;
            OnDisplayDone();
        }

        public void DisplayComment(Comment comment)
        {
            messageBox = Instantiate(messageBoxPrefab.gameObject, messageContainer).GetComponent<ChatBubbleUI>();
            messageBox.transform.SetAsLastSibling();
            messageBox.Set(messageBoxSettings);

            messageBox.SetText(comment.text.GetLocalizedString(), "@" + comment.handle);
        }

        public void OnDisplayDone()
        {
            if (appNavbarUI != null && lockNavbar) appNavbarUI.SetNavbarEnabled(true);
        }

        public override void Clear()
        {
            base.Clear();
            //Debug.Log("Comments cleared.");
            if (commentsQueue != null) commentsQueue.Clear();
            /*while (anchor.childCount > 0)
            {
                Destroy(anchor.GetChild(anchor.childCount - 1).gameObject);
            }*/
        }
    }
}
