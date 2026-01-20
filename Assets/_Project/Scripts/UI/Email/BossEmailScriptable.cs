using System.Collections;
using System.Collections.Generic;
using Mystie.Core;
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

        [SerializeField] private LocalizedString trendingPositive;
        [SerializeField] private LocalizedString trendingNegative;
        [SerializeField] private LocalizedString stylePositive;
        [SerializeField] private LocalizedString styleNegative;

        [Header("Body Old")]

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
                string signatureText = "";

                bool style = LevelManager.Instance.dressup.CheckStyleRule();
                bool trending = LevelManager.Instance.dressup.CheckTrending();

                if (style && trending)
                {
                    bodyText += bodyPositive.GetLocalizedString();
                    signatureText += "\n\n" + signaturePositive.GetLocalizedString();
                }
                else if (style)
                {
                    bodyText += bodyStyle.GetLocalizedString();
                    signatureText += "\n\n" + signatureNeutral.GetLocalizedString();
                }
                else if (trending)
                {
                    bodyText += bodyTrending.GetLocalizedString();
                    signatureText += "\n\n" + signatureNeutral.GetLocalizedString();
                }
                else
                {
                    bodyText += bodyNegative.GetLocalizedString();
                    signatureText += "\n\n" + signatureBad.GetLocalizedString();
                }

                string revenueText = $"Revenue: {LevelManager.Instance.GetRevenue()}$";

                return bodyText + "\n\n" + revenueText + "\n\n" + signatureText;
            }
        }
    }
}
