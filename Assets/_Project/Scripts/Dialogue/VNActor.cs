using UnityEngine;

namespace Mystie
{
    /// <summary>
    /// stores data for actors (sprite reference and color), can be
    /// expanded if necessary
    /// </summary>
    [System.Serializable]
    public class VNActor
    {
        public SpriteLayered actorImage;
        public Color actorColor;
        public RectTransform rectTransform { get { return actorImage.rectTransform; } }
        public GameObject gameObject { get { return actorImage.gameObject; } }

        public VNActor(SpriteLayered actorImage, Color actorColor)
        {
            this.actorImage = actorImage;
            this.actorColor = actorColor;
        }
    }
}
