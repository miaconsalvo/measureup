using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Actor", menuName = "Data/Visual Novel/Actor", order = 1)]
    public class ActorScriptable : ScriptableObject
    {
        public List<SpriteItem> poses;
        public List<SpriteItem> expressions;

        public Sprite[] Get(string pose, string expression)
        {
            string[] spriteParams = { pose, expression };
            return Get(spriteParams);
        }

        public Sprite[] Get(string[] spriteParams)
        {
            Sprite[] sprites = new Sprite[spriteParams.Length];
            List<List<SpriteItem>> lists = new List<List<SpriteItem>>() { poses, expressions };

            for (int i = 0; i < spriteParams.Length; i++)
            {
                List<SpriteItem> list = lists[i];
                SpriteItem s = list.Find(s => s.name == spriteParams[i]);
                if (s == null) s = list[0];
                Sprite sprite = s != null ? s.sprite : null;
                sprites[i] = sprite;
            }

            return sprites;
        }
    }
}
