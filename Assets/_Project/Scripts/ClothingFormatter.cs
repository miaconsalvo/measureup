using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mystie.Core;
using Mystie.Dressup;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Core.Extensions;

namespace Mystie
{
    public class ClothingFormatter : FormatterBase
    {
        public const string NOT = "!";

        public override string[] DefaultNames => new[] { "clothing", "c" };

        public override bool TryEvaluateFormat(IFormattingInfo formattingInfo)
        {
            List<ItemScriptable> items = (List<ItemScriptable>)formattingInfo.CurrentValue;

            string targetTags = formattingInfo.FormatterOptions;
            List<string> targetTagsList = targetTags.Split(',').ToList();

            // Get the tag from the formatter options
            if (targetTagsList.IsNullOrEmpty())
            {
                formattingInfo.Write("[No clothing tag specified]");
                return true;
            }

            if (!items.IsNullOrEmpty())
            {
                foreach (ItemScriptable item in items.Shuffle())
                {
                    if (targetTagsList.All(t => !t.StartsWith(NOT) ?
                        item.TagsStrings.Contains(t) : !item.TagsStrings.Contains(t)))
                    {
                        formattingInfo.Write(item.displayName.GetLocalizedString().ToLower());
                        return true;
                    }

                    //if (targetTagsList.Any(item.TagsStrings.Contains)) //OR operator
                }
            }

            formattingInfo.Write($"[No item with tags '{targetTags}' found]");

            return true;
        }
    }
}


//
