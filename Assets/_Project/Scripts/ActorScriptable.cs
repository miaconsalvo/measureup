using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Actor", menuName = "Data/Visual Novel/Actor", order = 1)]
    public class ActorScriptable : ScriptableObject
    {
        public string name;
        public string code;
        public List<SpriteItem> poses;
        public List<SpriteItem> expressions;

        public Sprite[] Get(string[] spriteParams)
        {
            (Sprite[] sprites, Vector2 _) = GetSprites(spriteParams);
            return sprites;
        }

        public Sprite[] Get(string pose, string expression)
        {
            string[] spriteParams = { pose, expression };
            return Get(spriteParams);
        }

        public (Sprite[] sprites, Vector2 offset) GetSprites(string[] spriteParams)
        {
            Sprite[] sprites = new Sprite[spriteParams.Length];
            Vector2 offset = Vector2.zero;
            List<List<SpriteItem>> lists = new List<List<SpriteItem>>() { poses, expressions };

            for (int i = 0; i < spriteParams.Length; i++)
            {
                List<SpriteItem> list = lists[i];
                SpriteItem s = list.Find(s => s.name == spriteParams[i]);
                if (s == null) s = list[0];

                sprites[i] = s?.sprite;
                if (i == 0)
                {
                    offset = s?.offset ?? Vector2.zero;
                    if (i == 0) offset = s?.offset ?? Vector2.zero;

                }
            }
            return (sprites, offset);
        }

        public void Set(SpriteLayered sprite, string[] spriteParams)
        {
            (Sprite[] sprites, Vector2 offset) = GetSprites(spriteParams);
            sprite.Set(sprites, offset);
        }
    }
}
