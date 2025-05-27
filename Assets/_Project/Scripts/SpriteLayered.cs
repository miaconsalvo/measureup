using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie
{
    public class SpriteLayered : MonoBehaviour
    {
        public RectTransform rectTransform { get; private set; }

        public Color color = Color.white;
        public List<SpriteRenderer> renderers;
        public List<Image> renderersUI;

        public void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            UpdateColors();
        }

        public void Set(Sprite sprite)
        {
            Set(new Sprite[] { sprite });
        }

        public void Set(Sprite[] sprites)
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].sprite = i < sprites.Length ? sprites[i] : null;
                renderers[i].enabled = renderers[i].sprite != null;
            }

            for (int i = 0; i < renderersUI.Count; i++)
            {
                renderersUI[i].sprite = i < sprites.Length ? sprites[i] : null;
                renderersUI[i].enabled = renderersUI[i].sprite != null;
            }
        }

        public void UpdateColors()
        {
            foreach (Image image in renderersUI)
            {
                if (image != null) image.color = color;
            }

            foreach (SpriteRenderer sprite in renderers)
            {
                if (sprite != null) sprite.color = color;
            }
        }

        public void SetNativeSize()
        {
            for (int i = 0; i < renderersUI.Count; i++)
            {
                renderersUI[i].SetNativeSize();
                if (i > 0)
                {
                    renderersUI[i].rectTransform.anchorMin = Vector2.one / 2;
                    renderersUI[i].rectTransform.anchorMax = Vector2.one / 2;
                    renderersUI[i].rectTransform.anchoredPosition = new Vector2(0, 0);
                }
            }

        }

        public void OnValidate()
        {
            UpdateColors();
        }

        public void Reset()
        {
            renderers = GetComponentsInChildren<SpriteRenderer>().ToList();
            renderersUI = GetComponentsInChildren<Image>().ToList();
        }
    }
}
