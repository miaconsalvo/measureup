using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie
{
    [RequireComponent(typeof(Toggle), typeof(Animator))]
    public class ToggleAnimator : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private string isOnParam = "on";
        [SerializeField] private Animator animator;

        private void Awake()
        {
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDestroy()
        {
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool value)
        {
            animator.SetBool(isOnParam, value);
        }

        private void Reset()
        {
            toggle = GetComponentInChildren<Toggle>();
            animator = GetComponentInChildren<Animator>();
        }
    }
}
