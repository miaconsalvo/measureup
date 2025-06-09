using System.Collections;
using System.Collections.Generic;
using Mystie.Dressup;
using UnityEngine;
using UnityEngine.Localization;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Email", menuName = "Data/Email/Boss Email", order = 1)]
    public class BossEmailScriptable : EmailScriptable
    {
        [SerializeField] private LocalizedString subject;

        [Header("Body")]

        [SerializeField] private LocalizedString bodyPositive;
        [SerializeField] private LocalizedString bodyStyle;
        [SerializeField] private LocalizedString bodyTrending;
        [SerializeField] private LocalizedString bodyNegative;

        [Header("Rating")]

        [SerializeField] private LocalizedString ratingPositive;
        [SerializeField] private LocalizedString ratingNeutral;
        [SerializeField] private LocalizedString ratingBad;

        [Header("Signature")]

        [SerializeField] private LocalizedString signaturePositive;
        [SerializeField] private LocalizedString signatureNeutral;
        [SerializeField] private LocalizedString signatureBad;

        public override string Subject => subject.GetLocalizedString();
        public override string Body
        {
            get
            {
                string bodyText = "";

                bool style = DressupManager.Instance.CheckStyleRule();
                bool trending = DressupManager.Instance.CheckTrending();

                if (style && trending)
                {
                    bodyText += bodyPositive.GetLocalizedString();
                    bodyText += "\n\n" + signaturePositive.GetLocalizedString();
                }
                else if (style)
                {
                    bodyText += bodyStyle.GetLocalizedString();
                    bodyText += "\n\n" + signatureNeutral.GetLocalizedString();
                }
                else if (trending)
                {
                    bodyText += bodyTrending.GetLocalizedString();
                    bodyText += "\n\n" + signatureNeutral.GetLocalizedString();
                }
                else
                {
                    bodyText += bodyNegative.GetLocalizedString();
                    bodyText += "\n\n" + signatureBad.GetLocalizedString();
                }

                return bodyText;
            }
        }
    }
}
