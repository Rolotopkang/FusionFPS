using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
       #region FIELDS SERIALIZED

        [Header("Settings")]

        [Tooltip("Display name of the scene.")]
        [SerializeField]
        private string displayName;
        
        [Tooltip("Name of the scene to load.")]
        [SerializeField]
        private string sceneToLoad;
        
        [Tooltip("Loading Screen Object.")]
        [SerializeField]
        private GameObject loadingScreen;
        
        [Tooltip("Canvas Group.")]
        [SerializeField]
        private CanvasGroup canvasGroup;
        
        [Tooltip("Scene Text.")]
        [SerializeField]
        private TMP_Text sceneText;

        [Tooltip("Duration of the fade.")]
        [SerializeField]
        public float fadeDuration = 1.0f;
        
        #endregion
        
        #region UNITY

        private void Start()
        {
            //Make sure the canvas group alpha is set to 0 initially.
            canvasGroup.alpha = 0;
        }

        public void OnBTNChangeScene()
        {
            //Start loading if the player is in the zone!
            StartCoroutine(LoadScene());
        }
        
        #endregion

        #region METHODS

        /// <summary>
        /// Load the level!
        /// </summary>
        private IEnumerator LoadScene()
        {
            //Activate the UI object.
            loadingScreen.SetActive(true);
            
            //Display Name.
            sceneText.text = displayName;

            //Fade in loading screen.
            yield return StartCoroutine(FadeLoadingScreen(1, fadeDuration));

            //Operation.
            AsyncOperation operation = default;

            #if UNITY_EDITOR
            //Load the scene.
            operation = EditorSceneManager.LoadSceneAsyncInPlayMode(sceneToLoad, new LoadSceneParameters(LoadSceneMode.Single));
            #else
            //Load the scene.
            operation = SceneManager.LoadSceneAsync(sceneToLoad, new LoadSceneParameters(LoadSceneMode.Single));
            #endif
            
            //Yield.
            yield return new WaitWhile(() => !operation.isDone);

            //Fade out loading screen once loading is completed.
            yield return StartCoroutine(FadeLoadingScreen(0, fadeDuration));

            //Disable the game object so it doesn't show up in the loaded scene.
            // gameObject.SetActive(false);
        }

        /// <summary>
        /// Loading Screen Fade.
        /// </summary>
        private IEnumerator FadeLoadingScreen(float targetValue, float duration)
        {
            float startValue = canvasGroup.alpha;
            float time = 0;

            while (time < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = targetValue;
        }

        #endregion
}
