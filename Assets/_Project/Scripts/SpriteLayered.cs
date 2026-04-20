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

            List<Image> copyOrdered = copy.renderersUI
                .Where(r => r != null)
                .OrderBy(r => r.transform.GetSiblingIndex())
                .ToList();

            List<Image> selfOrdered = renderersUI
                .Where(r => r != null)
                .OrderBy(r => r.transform.GetSiblingIndex())
                .ToList();

            // Apply sibling ordering to self before copying
            for (int i = 0; i < selfOrdered.Count; i++)
            {
                selfOrdered[i].transform.SetSiblingIndex(
                    i < copyOrdered.Count
                        ? copyOrdered[i].transform.GetSiblingIndex()
                        : selfOrdered[i].transform.GetSiblingIndex()
                );
            }

            // Get the sprites from the copy object
            int count = Mathf.Min(renderersUI.Count, copyOrdered.Count);

            for (int i = 0; i < selfOrdered.Count; i++)
            {
                if (i < count && copyOrdered[i] != null)
                {
                    // Copy the sprite from the corresponding renderer
                    selfOrdered[i].sprite = copyOrdered[i].sprite;
                    selfOrdered[i].enabled = copyOrdered[i].enabled;

                    if (selfOrdered[i].sprite != null)
                    {
                        selfOrdered[i].SetNativeSize();

                        int selfIdx = renderersUI.IndexOf(selfOrdered[i]);
                        int copyIdx = copy.renderersUI.IndexOf(copyOrdered[i]);

                        // Calculate offset from copy's base position
                        Vector2 basePos = selfIdx < basePositions.Count ? basePositions[i] : Vector2.zero;
                        Vector2 copyBasePos = copyIdx < copy.basePositions.Count ? copy.basePositions[i] : Vector2.zero;
                        Vector2 offset = copyOrdered[i].rectTransform.anchoredPosition - copyBasePos;

                        selfOrdered[i].rectTransform.anchoredPosition = selfIdx > 0 ? basePos + offset : basePos;
                    }
                }
                else
                {
                    // Clear any extra renderers that don't have a corresponding copy
                    renderersUI[i].sprite = null;
                    renderersUI[i].enabled = false;
                }
            }

            SetNativeSize();
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

            SetNativeSize();
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
                if (renderersUI[i] != null) renderersUI[i].SetNativeSize();
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
