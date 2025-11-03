using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Mystie.UI
{
    public class EmailUI : AppUI
    {
        [SerializeField] private List<EmailScriptable> emails;

        [SerializeField] private EmailPreviewUI emailUI;
        [SerializeField] private Transform emailPreviewsAnchor;
        [SerializeField] private Transform emailDisplay;

        [SerializeField] private TextMeshProUGUI emailSender;
        [SerializeField] private TextMeshProUGUI emailSubject;
        [SerializeField] private TextMeshProUGUI emailBody;

        private EmailScriptable currentEmail;

        public void Start()
        {
            GenerateEmails();
            OpenEmail(null);
        }

        public void GenerateEmails()
        {
            for (int i = emailPreviewsAnchor.childCount - 1; i >= 0; i--)
                Destroy(emailPreviewsAnchor.GetChild(i).gameObject);

            foreach (EmailScriptable email in emails)
            {
                if (email == null) continue;
                EmailPreviewUI emailPreview = Instantiate(emailUI.gameObject, emailPreviewsAnchor).GetComponent<EmailPreviewUI>();
                emailPreview.SetText(email.Subject, email.sender);
                emailUI.SetRead(false);
                emailPreview.button.onClick.AddListener(() => OpenEmail(emailPreview, email));
            }
        }

        [Button()]
        public void OpenEmail()
        {
            OpenEmail(emails[0]);
        }

        public void OpenEmail(EmailPreviewUI emailUI, EmailScriptable email)
        {
            emailUI.SetRead(true);
            OpenEmail(email);
        }

        public void OpenEmail(EmailScriptable email)
        {
            currentEmail = email;

            if (currentEmail == null)
            {
                emailDisplay.gameObject.SetActive(false);
                return;
            }

            emailDisplay.gameObject.SetActive(true);
            emailSender.text = currentEmail.sender;
            emailSubject.text = currentEmail.Subject;
            emailBody.text = currentEmail.Body;
        }
    }
}
