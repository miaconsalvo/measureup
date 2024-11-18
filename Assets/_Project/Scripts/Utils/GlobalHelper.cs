using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie.Core
{
    public static class GlobalHelper
    {
        // Function to find the GCD (Greatest Common Divisor) of two numbers
        public static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        // Function to find the LCM (Least Common Multiple) of two numbers
        public static int LCM(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            // Ensure a and b are positive numbers
            //if (a < 0 || b < 0)
            //throw new ArgumentException("Inputs must be non-negative.");

            // Calculate LCM using the formula LCM(a, b) = (a * b) / GCD(a, b)
            return (a * b) / GCD(a, b);
        }

        public static float Ease(this float x, float a)
        {
            return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
        }

        public static Color GetColor(float r, float g, float b, float a)
        {
            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }
    }
}
