using System.Collections;
using System.Collections.Generic;
using Mystie.Dressup;
using UnityEngine;

namespace Mystie
{
    public class TagLabelUI : LabelUI
    {
        public ClothingTag tag { get; private set; }

        public void Set(ClothingTag newTag)
        {
            tag = newTag;
            Set(tag.displayName, tag.color, tag.sprite);
        }
    }
}
