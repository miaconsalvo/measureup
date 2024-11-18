using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie.Utils
{
    public struct RectData
    {
        float h;
        float w;

        float anchorX;
        float anchorY;

        float gapX;
        float gapY;

        public RectData(float _w, float _h, float _anchorX, float _anchorY, float _gapX, float _gapY)
        {
            w = _w;
            h = _h;
            anchorX = _anchorX;
            anchorY = _anchorY;
            gapX = _gapX;
            gapY = _gapY;
        }

        public Rect GetRect(int i = 0)
        {
            return new Rect(anchorX + i * (gapX), anchorY + i * (gapY), w, h);
        }
    }
}
