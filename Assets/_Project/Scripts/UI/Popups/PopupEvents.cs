using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Mystie
{
    public class PopupEvents : MonoBehaviour
    {
        public static event Action<LocalizedString, Action, Action> OnConfirmationRequested;

        public static void RequestConfirmation(LocalizedString message, Action onConfirm, Action onCancel = null)
        {
            OnConfirmationRequested?.Invoke(message, onConfirm, onCancel);
        }
    }
}
