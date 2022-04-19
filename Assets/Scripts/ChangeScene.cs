using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityTemplateProjects.Tools;

public class ChangeScene : Singleton<ChangeScene>
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
        
        
        private int mapIndex = -1;
        
        private MapTools.GameMode gameMode;

        private bool isNetWork =false;

        private bool isNetWorkClient = false;

        private int NetWorkSceneIndex;

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

            if (mapIndex != -1)
            {
                MapTools.Map tmp_map = MapTools.IndexToMap(mapIndex,gameMode);
                displayName = tmp_map.MapDiscripName;
                sceneToLoad = tmp_map.MapName;
                NetWorkSceneIndex = tmp_map.SceneIndex;
            }

                //Display Name.
            sceneText.text ="Connecting   " + displayName;

            //Fade in loading screen.
            yield return StartCoroutine(FadeLoadingScreen(1, fadeDuration));

            //Operation.
            AsyncOperation operation = default;


            if (isNetWork)
            {
                PhotonNetwork.LoadLevel(sceneToLoad);
            }
            else if (isNetWorkClient)
            {
                PhotonNetwork.LoadLevel(NetWorkSceneIndex);
            }
            else
            {
                //Load the scene.
                operation = SceneManager.LoadSceneAsync(sceneToLoad, new LoadSceneParameters(LoadSceneMode.Single));
            }
            

            
            //Yield.
            yield return new WaitWhile(() =>isNetWork||isNetWorkClient?PhotonNetwork.LevelLoadingProgress.Equals(1): !operation.isDone);

            // //Fade out loading screen once loading is completed.
            // yield return StartCoroutine(FadeLoadingScreen(0, fadeDuration));
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

        public void SetMapToChange(int index ,MapTools.GameMode GameMode)
        {
            mapIndex = index;
            gameMode = GameMode;
            isNetWork = true;
        }

        public void SetMapToChange(int index ,string DisplayName)
        {
            NetWorkSceneIndex = index;
            displayName = DisplayName;
            isNetWorkClient = true;
        }
        #endregion
}
