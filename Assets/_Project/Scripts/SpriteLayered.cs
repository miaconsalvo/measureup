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

        public void Set(SpriteLayered copy)
        {
            if (copy == null || copy.renderersUI == null) return;

            // Get the sprites from the copy object
            int count = Mathf.Min(renderersUI.Count, copy.renderersUI.Count);

            for (int i = 0; i < renderersUI.Count; i++)
            {
                if (i < count && copy.renderersUI[i] != null)
                {
                    // Copy the sprite from the corresponding renderer
                    renderersUI[i].sprite = copy.renderersUI[i].sprite;
                    renderersUI[i].enabled = copy.renderersUI[i].enabled;

                    if (renderersUI[i].sprite != null)
                    {
                        renderersUI[i].SetNativeSize();

                        // Calculate offset from copy's base position
                        Vector2 basePos = i < basePositions.Count ? basePositions[i] : Vector2.zero;
                        Vector2 copyBasePos = i < copy.basePositions.Count ? copy.basePositions[i] : Vector2.zero;
                        Vector2 offset = copy.renderersUI[i].rectTransform.anchoredPosition - copyBasePos;

                        if (i > 0)
                            renderersUI[i].rectTransform.anchoredPosition = basePos + offset;
                        else
                            renderersUI[i].rectTransform.anchoredPosition = basePos;
                    }
                }
                else
                {
                    // Clear any extra renderers that don't have a corresponding copy
                    renderersUI[i].sprite = null;
                    renderersUI[i].enabled = false;
                }
            }

            UpdateColors();
        }

        public void Set(Sprite[] sprites, Vector2 offset = default, bool overwrite = true)
        {
            UpdateColors();

            for (int i = 0; i < renderersUI.Count; i++)
            {
                if (overwrite)
                {
                    renderersUI[i].sprite = i < sprites.Length ? sprites[i] : null;
                    renderersUI[i].enabled = renderersUI[i].sprite != null;
                }
                else
                {
                    if (i < sprites.Length) renderersUI[i].sprite = sprites[i];
                    if (renderersUI[i].sprite != null) renderersUI[i].enabled = true;
                }

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
            renderersUI = GetComponentsInChildren<Image>().ToList();
        }
    }
}
