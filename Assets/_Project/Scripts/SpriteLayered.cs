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

        private List<Vector2> basePositions = new List<Vector2>();

        public void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            StoreBasePositions();
        }

        private void StoreBasePositions()
        {
            basePositions.Clear();
            foreach (Image image in renderersUI)
            {
                basePositions.Add(image.rectTransform.anchoredPosition);
            }
        }

        private void Update()
        {
            UpdateColors();
        }

        public void Set(Sprite sprite)
        {
            Set(new Sprite[] { sprite });
        }

        public void Set(Sprite[] sprites, Vector2 offset = default)
        {
            UpdateColors();

            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].sprite = i < sprites.Length ? sprites[i] : null;
                renderers[i].enabled = renderers[i].sprite != null;

                if (i > 0)
                {
                    renderers[i].transform.localPosition = offset;
                }
                else renderers[i].transform.localPosition = Vector3.zero;
            }

            for (int i = 0; i < renderersUI.Count; i++)
            {
                renderersUI[i].sprite = i < sprites.Length ? sprites[i] : null;
                renderersUI[i].enabled = renderersUI[i].sprite != null;

                if (renderersUI[i].sprite != null)
                {
                    renderersUI[i].SetNativeSize();
                    Vector2 basePos = i < basePositions.Count ? basePositions[i] : Vector2.zero;

                    if (i > 0) renderersUI[i].rectTransform.anchoredPosition = basePos + offset;
                    else renderersUI[i].rectTransform.anchoredPosition = basePos;
                }
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
                /*
                if (i > 0)
                {
                    renderersUI[i].rectTransform.anchorMin = Vector2.one / 2;
                    renderersUI[i].rectTransform.anchorMax = Vector2.one / 2;
                    renderersUI[i].rectTransform.anchoredPosition = new Vector2(0, 0);
                }*/
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
