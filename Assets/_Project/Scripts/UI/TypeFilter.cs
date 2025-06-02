using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mystie.Dressup
{
    [Serializable]
    public class TypeFilter
    {
        public event Action onUpdate;

        [SerializeField] private List<ItemType> itemTypes = new List<ItemType>();
        public bool HasFilterTypes => itemTypes.Count > 0;

        public void Set(ItemType type)
        {
            itemTypes.Clear();
            itemTypes.Add(type);
            onUpdate?.Invoke();
        }

        public void Set(List<ItemType> types)
        {
            itemTypes = types;
            onUpdate?.Invoke();
        }

        public void Clear()
        {
            itemTypes.Clear();
            onUpdate?.Invoke();
        }

        public void ApplyFilter(ref Dictionary<ItemUI, bool> itemsUI)
        {
            foreach (ItemUI ui in itemsUI.Keys.ToList())
            {
                if (!itemsUI[ui]) continue;

                itemsUI[ui] = !HasFilterTypes || itemTypes.Contains(ui.item.type);
            }
        }
    }
}
