using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Mystie
{
    public class DragAndDropHandler : MonoBehaviour
    {
        public RectTransform pointer;
        public Image image;
        private bool isDragging;

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
        }


        void Update()
        {
            if (isDragging)
            {

            }
        }
    }
}
