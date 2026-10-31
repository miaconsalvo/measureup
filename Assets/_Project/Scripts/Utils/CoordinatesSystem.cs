using System.Globalization;
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

        private static string Normalize(string s)
        {
            return (s ?? string.Empty).ToLowerInvariant().Replace(" ", "").Replace("_", "").Replace("-", "");
        }

        public float GetCoordinates(string coordinate)
        {
            // next, let's see if they used a position keyword
            string labelCoordinate = Normalize(coordinate);

            // Normalise the inspector keys too, so "Top Left" or "top_left" still match.
            foreach (var kvp in labelCoordinates)
            {
                if (Normalize(kvp.Key) == labelCoordinate)
                    return kvp.Value;
            }

            // if none of those worked, then let's try parsing it as a
            // number
            float x;
            if (float.TryParse(coordinate, NumberStyles.Float, CultureInfo.InvariantCulture, out x))
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
