using UnityEngine;

namespace Mystie
{
    [CreateAssetMenu(fileName = "Save Data", menuName = "Data/System/Save Data")]
    public class SaveDataScriptable : ScriptableObject
    {
        [field: SerializeField] public string playerName { get; private set; } = "Cindy";
        [field: SerializeField] public int startingMoney { get; private set; } = 1000;
    }
}
