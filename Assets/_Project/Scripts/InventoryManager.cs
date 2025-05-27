using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie.Dressup
{
    public class InventoryManager : MonoBehaviour
    {
        [field: SerializeField] public List<ItemScriptable> clothes { get; private set; }

        #region Singleton

        private static InventoryManager instance;
        public static InventoryManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<InventoryManager>();

                return instance;
            }
        }

        #endregion

        public void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
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
