using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mystie
{
    public class DraggableTest : MonoBehaviour, IBeginDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("Drag detected!");
        }
    }
}
