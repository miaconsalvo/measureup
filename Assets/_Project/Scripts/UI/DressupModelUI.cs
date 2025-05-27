using System.Collections;
using System.Collections.Generic;
using Mystie.Dressup;
using UnityEngine;

namespace Mystie
{
    public class DressupModelUI : MonoBehaviour
    {
        [SerializeField] private DressupModel model;

        public void AddItem(ItemScriptable item)
        {
            if (item == null) return;
            model.AddItem(item);
        }
    }
}
