using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie.Dressup
{
    public class InventoryManager : MonoBehaviour
    {
        [field: SerializeField] public List<ItemScriptable> clothes { get; private set; }

        public void Initialize(EpisodeScriptable episode)
        {
            AddClothes(episode.startingClothes);
        }

        public void AddClothes(List<ItemScriptable> newClothes)
        {
            clothes.AddRange(newClothes);
        }

        public void AddClothes(ClothingKitScriptable newClothingKit)
        {
            Debug.Log("Added kit " + newClothingKit.ToString());
            //foreach (ItemScriptable garment in newClothingKit.garments)
            //Debug.Log(garment.ToString());
            AddClothes(newClothingKit.garments);
        }
    }
}
