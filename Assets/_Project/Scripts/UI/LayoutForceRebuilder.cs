using UnityEngine;
using UnityEngine.UI;

namespace Mystie.UI
{
    public class LayoutForceRebuilder : MonoBehaviour
    {
        private RectTransform _rect;
        private int _lastActiveChildCount = -1;

        void Awake() => _rect = GetComponent<RectTransform>();

        void LateUpdate()
        {
            int activeCount = 0;
            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).gameObject.activeSelf) activeCount++;

            if (activeCount != _lastActiveChildCount)
            {
                _lastActiveChildCount = activeCount;
                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);
            }
        }
    }
}
