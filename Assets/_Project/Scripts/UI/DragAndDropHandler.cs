using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mystie
{
    public class DragAndDropHandler : MonoBehaviour
    {
        private bool isDragging;

        private Canvas canvas;
        [SerializeField] private GameObject dragVisual;
        private RectTransform dragVisualRect;
        private Image dragVisualImage;

        private object dragSource;
        private object dragData;
        private System.Action<object, GameObject> onDropCallback;

        private static DragAndDropHandler instance;
        public static DragAndDropHandler Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<DragAndDropHandler>();
                return instance;
            }
        }

        void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            canvas = GetComponentInParent<Canvas>();
            dragVisualRect = dragVisual.GetComponent<RectTransform>();
            dragVisualImage = dragVisual.GetComponent<Image>();
        }


        void Update()
        {
            if (isDragging) UpdateDragPosition();
        }

        public void StartDrag(object source, object data, Sprite sprite, Vector2 size,
                        System.Action<object, GameObject> dropCallback)
        {
            if (isDragging) return;

            dragSource = source;

            dragVisualImage.enabled = true;
            dragVisualImage.sprite = sprite;
            dragVisualImage.raycastTarget = false;

            dragVisualRect.sizeDelta = size;

            dragData = data;
            onDropCallback = dropCallback;

            dragVisual.SetActive(true);

            isDragging = true;
        }

        public void EndDrag(object source)
        {
            if (!isDragging || source != dragSource) return;

            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            GameObject dropTarget = null;
            foreach (RaycastResult result in results)
            {
                if (result.gameObject != dragVisual)
                {
                    dropTarget = result.gameObject;
                    break;
                }
            }

            // Invoke callback
            onDropCallback?.Invoke(dragData, dropTarget);

            isDragging = false;
            dragSource = null;
            dragData = null;
            onDropCallback = null;

            dragVisual.SetActive(false);
            dragVisualImage.enabled = false;
            dragVisualImage.sprite = null;
        }

        private void UpdateDragPosition()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out position);

            dragVisualRect.localPosition = position;
        }
    }
}
