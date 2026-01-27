using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mystie.Core;
using Mystie.Dialogue;
using Mystie.Dressup;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Utilities;
using UnityEngine.UI;
using VInspector;

namespace Mystie
{
    public class DressupModelUI : MonoBehaviour
    {
        private DressupManager dressupManager;
        //[SerializeField] private Image clothingImage;
        //[SerializeField] private Transform modelAnchor;
        //[SerializeField] private Image clothingImage;
        //[SerializeField] private List<ItemType> layerOrder;
        [field: SerializeField] public SpriteLayered model { get; private set; }
        //[SerializeField] private SpriteLayered dialogueModel;
        [SerializeField] private Image modelImage;

        [SerializeField] private SerializedDictionary<ItemType, Image> itemsUI;
        [SerializeField] private SerializedDictionary<ItemType, Image> itemsBgUI;

        public void Initialize(EpisodeScriptable episode)
        {
            dressupManager = LevelManager.Instance.dressup;

            modelImage.sprite = episode.contestant.model;

            dressupManager.onItemAdded += OnItemAdded;
            dressupManager.onItemRemoved += OnItemRemoved;
        }

        private void Start()
        {
            //itemsUI = new Dictionary<ItemType, Image>();
            //clothingImage.gameObject.SetActive(false);

            //InitializeLayers();
        }

        private void OnDestroy()
        {
            if (dressupManager != null)
            {
                dressupManager.onItemAdded -= OnItemAdded;
                dressupManager.onItemRemoved -= OnItemRemoved;
            }
        }

        /*public void AddItem(ItemScriptable item)
        {
            if (item == null) return;
            dressupManager.AddItem(item);
        }*/

        /*public void InitializeLayers()
        {
            foreach (ItemType type in layerOrder)
            {
                Image img = Instantiate(clothingImage.gameObject, modelAnchor).GetComponent<Image>();
                itemsUI.Add(type, img);
            }
        }*/

        public void OnItemAdded(ItemScriptable item)
        {
            if (item == null) return;

            OnItemRemoved(item.type);

            /*if (!itemsUI.ContainsKey(item.type))
            {
                Image img = Instantiate(clothingImage.gameObject, modelAnchor).GetComponent<Image>();
                itemsUI.Add(item.type, img);
            }*/

            if (itemsUI.ContainsKey(item.type))
            {
                itemsUI[item.type].gameObject.SetActive(item.sprite != null);
                itemsUI[item.type].sprite = item.sprite;
                itemsUI[item.type].SetNativeSize();
                //dialogueModel.Set(model);
            }

            if (itemsBgUI.ContainsKey(item.type))
            {
                itemsBgUI[item.type].gameObject.SetActive(item.spriteOutline != null);
                itemsBgUI[item.type].sprite = item.spriteOutline;
                itemsBgUI[item.type].SetNativeSize();
                //dialogueModel.Set(model);
            }

            ReorderItems();
        }

        public void OnItemRemoved(ItemScriptable item)
        {
            OnItemRemoved(item.type);
        }

        public void OnItemRemoved(ItemType itemType)
        {
            if (itemsUI.ContainsKey(itemType))
            {
                itemsUI[itemType].gameObject.SetActive(false);
                itemsUI[itemType].sprite = null;
                //dialogueModel.Set(model);
            }

            if (itemsBgUI.ContainsKey(itemType))
            {
                itemsBgUI[itemType].gameObject.SetActive(false);
                itemsBgUI[itemType].sprite = null;
                //dialogueModel.Set(model);
            }

            ReorderItems();
        }

        private void ReorderItems()
        {
            List<ItemScriptable> sortedItems = dressupManager.items.Values
                .Where(item => item != null)
                .OrderBy(item => item.displayOrder)
                .ToList();

            for (int i = 0; i < sortedItems.Count; i++)
            {
                if (itemsUI.ContainsKey(sortedItems[i].type))
                {
                    itemsUI[sortedItems[i].type].transform.SetSiblingIndex(i);
                }
            }
        }
    }
}
