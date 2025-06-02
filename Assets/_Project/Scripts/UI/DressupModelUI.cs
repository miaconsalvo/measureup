using System.Collections;
using System.Collections.Generic;
using Mystie.Dressup;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie
{
    public class DressupModelUI : MonoBehaviour
    {
        [SerializeField] private DressupManager model;
        [SerializeField] private Transform modelAnchor;
        [SerializeField] private Image clothingImage;

        private Dictionary<ItemType, Image> itemsUI;

        private void Awake()
        {
            itemsUI = new Dictionary<ItemType, Image>();
            clothingImage.gameObject.SetActive(false);

            model.onItemAdded += OnItemAdded;
            model.onItemRemoved += OnItemRemoved;
        }

        private void OnDestroy()
        {
            model.onItemAdded -= OnItemAdded;
            model.onItemRemoved -= OnItemRemoved;
        }

        public void AddItem(ItemScriptable item)
        {
            if (item == null) return;
            model.AddItem(item);
        }

        public void OnItemAdded(ItemScriptable item)
        {
            if (item == null) return;

            if (!itemsUI.ContainsKey(item.type))
            {
                Image img = Instantiate(clothingImage.gameObject, modelAnchor).GetComponent<Image>();
                itemsUI.Add(item.type, img);
            }

            itemsUI[item.type].gameObject.SetActive(item.sprite != null);
            itemsUI[item.type].sprite = item.sprite;
            //itemsUI[item.type].SetNativeSize();
        }

        public void OnItemRemoved(ItemScriptable item)
        {
            if (itemsUI.ContainsKey(item.type))
            {
                itemsUI[item.type].gameObject.SetActive(false);
                itemsUI[item.type].sprite = null;
            }
        }
    }
}
