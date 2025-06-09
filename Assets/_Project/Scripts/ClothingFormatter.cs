using System.Collections;
using System.Collections.Generic;
using Mystie.Dressup;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat;
using UnityEngine.Localization.SmartFormat.Core.Extensions;

namespace Mystie
{
    public class ClothingFormatter : FormatterBase
    {
        public override string[] DefaultNames => new[] { "clothing" };

        public override bool TryEvaluateFormat(IFormattingInfo formattingInfo)
        {
            if (formattingInfo.CurrentValue is string tag)
            {
                string clothing = DressupManager.GetItemWithTag(tag);
                formattingInfo.Write(clothing);
                return true;
            }

            return false;
        }
    }
}
