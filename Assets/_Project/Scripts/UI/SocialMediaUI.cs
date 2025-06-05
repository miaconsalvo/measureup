using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Mystie.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.UI
{
    public class SocialMediaUI : MonoBehaviour
    {
        [SerializeField] private RectTransform messageContainer;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private MessageBoxUI messageBoxPrefab;
        [SerializeField] private MessageBoxSettings messageBoxSettings;
        [SerializeField] private float delayBetweenPostsMin = 1f;
        [SerializeField] private float delayBetweenPostsMax = 2f;
        [SerializeField] private EventReference postSFX;

        private MessageBoxUI messageBox;
        private Queue<Comment> commentsQueue;
        private bool displayingComments;

        public void QueueComments(List<Comment> comments)
        {
            if (comments.IsNullOrEmpty()) return;

            if (commentsQueue == null) commentsQueue = new Queue<Comment>();

            foreach (Comment comment in commentsQueue)
                commentsQueue.Enqueue(comment);

            if (!displayingComments) StartCoroutine(DisplayComments());
        }

        public IEnumerator DisplayComments()
        {
            displayingComments = true;

            while (commentsQueue.Peek() != null)
            {
                DisplayComment(commentsQueue.Dequeue());

                commentsQueue.Dequeue();
                float delay = Random.Range(delayBetweenPostsMin, delayBetweenPostsMax);
                yield return new WaitForSeconds(delay);
            }

            displayingComments = false;
        }

        public void DisplayComment(Comment comment)
        {
            messageBox = Instantiate(messageBoxPrefab.gameObject, messageContainer).GetComponent<MessageBoxUI>();
            messageBox.transform.SetAsLastSibling();
            messageBox.Set(messageBoxSettings);

            messageBox.SetText("@" + comment.handle, comment.text.GetLocalizedString());
        }
    }
}
