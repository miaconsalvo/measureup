using Mystie.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mystie.MystEditor
{
    [CustomEditor(typeof(CreditsUI))]
    public class CreditsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            CreditsUI credits = (CreditsUI)target;
            credits.UpdateEntries();
        }
    }
}
