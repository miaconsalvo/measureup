using System;
using System.Collections.Generic;
using Mystie.Core;
using UnityEngine;

namespace Mystie
{
    public class EmailManager
    {
        public LevelManager levelManager { get; private set; }
        public EpisodeScriptable episode { get; private set; }
        public List<Email> emails;

        public void Initialize(EpisodeScriptable episode)
        {
            levelManager = LevelManager.Instance;
            this.episode = episode;

            emails = SaveDataManager.GetEmails();
            foreach (EmailScriptable email in episode.startEmails)
                if (email != null) AddEmail(email);

            levelManager.onStageComplete += OnStageComplete;
        }

        public void Clean()
        {
            levelManager.onStageComplete -= OnStageComplete;
        }

        private void OnStageComplete(LevelStageType type)
        {
            if (type == LevelStageType.CATWALK)
            {
                foreach (EmailScriptable email in episode.endEmails)
                    if (email != null) AddEmail(email);
            }
        }

        public void AddEmail(Email email)
        {
            if (emails == null || email == null) return;
            emails.Add(email);
        }

        public void AddEmail(EmailScriptable email)
        {
            if (emails == null || email == null) return;
            emails.Add(new Email(email));
        }
    }
}
