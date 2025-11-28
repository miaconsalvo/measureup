using UnityEngine;

namespace Mystie
{
    [System.Serializable]
    public class Email
    {
        public string sender;
        public string subject;
        public string date;
        public string body;
        public bool read;

        public Email(EmailScriptable email)
        {
            sender = email.sender;
            subject = email.Subject;
            body = email.Body;
            read = false;
        }
    }
}
