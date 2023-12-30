using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingManager: Singleton<PostProcessingManager>
{
    [System.Serializable]
    public class PostProcessing
    {
        public GameObject PP_Globle;
        public Volume PP_ScopeChanger;
        public GameObject PP_Weapon;
        public GameObject PP_Scope;
        public GameObject PP_UI;
        public GameObject PP_DeathZone;
    }
    
    [Header("后处理集合")]
    public PostProcessing PostProcessings;


    private Coroutine changingVolumeScopeChange;
    
    [Header("切换配件后处理设置")]
    public float duration;
    public AnimationCurve weightCurve;

    private void Start()
    {
        PostProcessings.PP_DeathZone.SetActive(false);
        PostProcessings.PP_ScopeChanger.gameObject.SetActive(true);
        PostProcessings.PP_ScopeChanger.weight = 0;
    }

    public void StartScopeChange(bool set)
    {
        if (set)
        {
            if (changingVolumeScopeChange!=null)
            {
                StopCoroutine(changingVolumeScopeChange);
                changingVolumeScopeChange = null;
                PostProcessings.PP_ScopeChanger.weight = 0;
            }
            changingVolumeScopeChange = StartCoroutine(IchangingVolumeScopeChange());
        }
        else
        {
            if (changingVolumeScopeChange!=null)
            {
                StopCoroutine(changingVolumeScopeChange);
                changingVolumeScopeChange = null;
                PostProcessings.PP_ScopeChanger.weight = 0;
            }
        }
    }

    private IEnumerator IchangingVolumeScopeChange()
    {
        float timer = 0f;
        float startWeight = PostProcessings.PP_ScopeChanger.weight;
        float targetWeight = 1.0f; // 目标权重值，你可以根据需要修改

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            float newWeight = Mathf.Lerp(startWeight, targetWeight, weightCurve.Evaluate(t));

            PostProcessings.PP_ScopeChanger.weight = newWeight;

            yield return null;
        }

        PostProcessings.PP_ScopeChanger.weight = targetWeight;
        
        
        // PostProcessings.PP_ScopeChanger.weight = Mathf.Lerp(0, 1, Time.deltaTime);
        // yield return new WaitUntil(() => PostProcessings.PP_ScopeChanger.weight >= 1);
    }
    
}
