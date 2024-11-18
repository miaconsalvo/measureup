using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.Core
{
    public static class Extensions
    {
        public static T Next<T>(this T src) where T : Enum
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }

        public static T Previous<T>(this T src) where T : Enum
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) - 1;
            return (j < 0) ? Arr[Arr.Length - 1] : Arr[j];
        }

        public static bool PlayAnimation(this Animator animator, string animation)
        {
            if (animator == null) return false;

            if (!animation.IsNullOrEmpty())
            {
                animator.Play(animation);
                animator.Update(0);
                return true;
            }

            return false;
        }

        public static void SetAlpha(this Image img, float a)
        {
            Mathf.Clamp01(a);

            Color stampColor = img.color;
            stampColor.a = a;
            img.color = stampColor;
        }

        public static void SetAlpha(this SpriteRenderer r, float a)
        {
            Mathf.Clamp01(a);

            Color stampColor = r.color;
            stampColor.a = a;
            r.color = stampColor;
        }

        public static void SetAlpha(this TextMeshPro txt, float a)
        {
            Mathf.Clamp01(a);

            Color stampColor = txt.color;
            stampColor.a = a;
            txt.color = stampColor;
        }
    }
}
