using System.Collections;
using System.Collections.Generic;
using Mystie.Core;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Mystie.UI
{
    public class EmailUI : AppUI
    {
        public EmailManager emailManager { get; private set; }

        [SerializeField] private EmailPreviewUI emailUI;
        [SerializeField] private Transform emailPreviewsAnchor;
        [SerializeField] private Transform emailDisplay;

        [SerializeField] private TextMeshProUGUI emailSender;
        [SerializeField] private TextMeshProUGUI emailSubject;
        [SerializeField] private TextMeshProUGUI emailBody;

        private List<Email> emails;
        private Email currentEmail;

        public void Start()
        {
            emailManager = LevelManager.Instance.emailManager;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            GenerateEmails();
            OpenEmail(null);
        }

        public void GenerateEmails()
        {
            for (int i = emailPreviewsAnchor.childCount - 1; i >= 0; i--)
                Destroy(emailPreviewsAnchor.GetChild(i).gameObject);

            emails = emailManager.emails;

            foreach (Email email in emailManager.emails)
            {
                if (email == null) continue;
                EmailPreviewUI emailPreview = Instantiate(emailUI.gameObject, emailPreviewsAnchor).GetComponent<EmailPreviewUI>();
                emailPreview.Set(email);
                emailPreview.button.onClick.AddListener(() => OpenEmail(emailPreview, email));
            }
        }

        [Button()]
        public void OpenEmail()
        {
            OpenEmail(emails[0]);
        }

        public void OpenEmail(EmailPreviewUI emailUI, Email email)
        {
            emailUI.SetRead(true);
            OpenEmail(email);
        }

        public void OpenEmail(Email email)
        {
            currentEmail = email;

            if (currentEmail == null)
            {
                emailDisplay.gameObject.SetActive(false);
                return;
            }

            emailDisplay.gameObject.SetActive(true);
            emailSender.text = currentEmail.sender;
            emailSubject.text = currentEmail.subject;
            emailBody.text = currentEmail.body;
        }
    }
}
