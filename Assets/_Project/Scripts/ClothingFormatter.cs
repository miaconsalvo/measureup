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

        private static bool MatchesTag(ItemScriptable item, string rawTag)
        {
            string tag = rawTag.Trim();
            // "!tag" means the item must NOT have that tag (the prefix has to be stripped first).
            if (tag.StartsWith(NOT)) return !item.TagsStrings.Contains(tag.Substring(NOT.Length));
            return item.TagsStrings.Contains(tag);
        }

        public override string[] DefaultNames => new[] { "clothing", "c" };

        public override bool TryEvaluateFormat(IFormattingInfo formattingInfo)
        {
            List<ItemScriptable> items = formattingInfo.CurrentValue as List<ItemScriptable>;

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
                    if (item == null) continue;

                    if (targetTagsList.All(t => MatchesTag(item, t)))
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
