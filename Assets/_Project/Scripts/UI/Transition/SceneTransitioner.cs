using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mystie.UI.Transition
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Canvas))]
    public class SceneTransitioner : MonoBehaviour
    {
        private static SceneTransitioner instance;

        public static SceneTransitioner Instance
        {
            get
            {
                if (instance == null) instance = Create();
                return instance;
            }
            private set => instance = value;
        }

        private const string sceneTransitionerPath = "Scene Transitioner";

        public static SceneTransitioner Create()
        {
            SceneTransitioner transitionerPrefab = Resources.Load<SceneTransitioner>(sceneTransitionerPath);
            if (transitionerPrefab == null) Debug.LogError("GameManager: Scene Transitioner not found.");
            return Instantiate(transitionerPrefab.gameObject).GetComponent<SceneTransitioner>();
        }

        private Canvas transitionCanvas;
        [SerializeField] private List<Transition> transitions = new();

        private AsyncOperation loadLevelOp;
        private AbstractSceneTransitionScriptable activeTransition;

        [SerializeField] private bool playTransitionOnStart;
        [SerializeField]
        [ShowIf("playTransitionOnStart")]
        private AbstractSceneTransitionScriptable startTransition;

        private void Awake()
        {
            /*if (Instance != null)
            {
                Debug.LogWarning("Invalid configuration. Duplicate Instances found! First one:" + Instance.name);
                Destroy(gameObject);
                return;
            }*/

            SceneManager.activeSceneChanged += HandleSceneChange;
            instance = this;
            DontDestroyOnLoad(gameObject);

            transitionCanvas = GetComponent<Canvas>();
            transitionCanvas.enabled = false;
        }

        private void Start()
        {
            if (playTransitionOnStart)
                PlayTransitionIn(startTransition);
        }

        public void PlayTransitionIn(AbstractSceneTransitionScriptable transition)
        {
            transitionCanvas.enabled = true;
            activeTransition = startTransition;
            StartCoroutine(EnterScene());
        }

        public void LoadScene(string scene, SceneTransitionMode transitionMode = SceneTransitionMode.Fade, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            Transition transition = transitions.Find((transition) => transition.mode == transitionMode);

            if (transition != null) LoadScene(scene, transition.animationScriptable, loadMode);
            else
            {
                Debug.LogWarning($"No transition found for TransitionMode {transitionMode}" +
                    $" Missing configuration.");
            }
        }

        public void LoadScene(string scene, AbstractSceneTransitionScriptable transition, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            if (transition == null)
            {
                Debug.LogWarning($"No transition set");
                return;
            }

            loadLevelOp = SceneManager.LoadSceneAsync(scene, loadMode);

            if (transition != null)
            {
                loadLevelOp.allowSceneActivation = false;
                transitionCanvas.enabled = true;
                activeTransition = transition;
                StartCoroutine(ExitScene());
            }
        }

        private IEnumerator ExitScene()
        {
            yield return StartCoroutine(activeTransition.Exit(transitionCanvas));
            loadLevelOp.allowSceneActivation = true;

        }

        private IEnumerator EnterScene()
        {
            yield return StartCoroutine(activeTransition.Enter(transitionCanvas));
            transitionCanvas.enabled = false;
            loadLevelOp = null;
            activeTransition = null;
        }

        private void HandleSceneChange(Scene oldScene, Scene newScene)
        {
            if (activeTransition != null)
            {
                StartCoroutine(EnterScene());
            }
        }
    }

    [System.Serializable]
    public class Transition
    {
        public SceneTransitionMode mode;
        public AbstractSceneTransitionScriptable animationScriptable;
    }
}
