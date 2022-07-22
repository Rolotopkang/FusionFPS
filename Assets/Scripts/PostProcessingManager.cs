using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessingManager: Singleton<PostProcessingManager>
{
    [System.Serializable]
    public class PostProcessing
    {
        public GameObject PP_Globle;
        public GameObject PP_Weapon;
        public GameObject PP_Scope;
        public GameObject PP_UI;
        public GameObject PP_DeathZone;
    }
    
    [Header("后处理集合")]
    public PostProcessing PostProcessings;

    private void Start()
    {
        PostProcessings.PP_DeathZone.SetActive(false);
    }
}
