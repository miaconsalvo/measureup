using UnityEngine;
using VInspector;

namespace Mystie
{
    [CreateAssetMenu(fileName = "CoordinatesSystem", menuName = "Data/Coordinates System")]
    public class CoordinatesSystem : ScriptableObject
    {
        // utility function to convert words like "left" or "right" into
        // equivalent position numbers

        public SerializedDictionary<string, float> labelCoordinates;

        public float GetCoordinates(string coordinate)
        {
            // next, let's see if they used a position keyword
            string labelCoordinate = coordinate.ToLower().Replace(" ", "").Replace("_", "").Replace("-", "");
            if (labelCoordinates.ContainsKey(labelCoordinate))
                return labelCoordinates[labelCoordinate];

            // if none of those worked, then let's try parsing it as a
            // number
            float x;
            if (float.TryParse(coordinate, out x))
            {
                return x;
            }
            else
            {
                Debug.LogErrorFormat(this, "Position [{0}] could not be converted... it must be an alignment (left, center, right, or top, middle, bottom) or a value (like 0.42 as 42%)", coordinate);
                return -1f;
            }
        }
    }
}
