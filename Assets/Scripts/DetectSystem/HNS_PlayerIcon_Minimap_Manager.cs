using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HNS_PlayerIcon_Minimap_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject FriendlyIcon;
    [SerializeField]
    private GameObject EnemylyIcon;

    private bool isFriendly;

    public void Initialisation(bool _isFriendly)
    {
        isFriendly = _isFriendly;
        //强制初始化
        SetIcon();
        EnemylyIcon.SetActive(false);
        FriendlyIcon.SetActive(_isFriendly);
    }
    
    
    public void SetIcon()
    {
        FriendlyIcon.SetActive(isFriendly);
        EnemylyIcon.SetActive(!isFriendly);
    }
    
    public void SetVisiable(bool set)
    {
        //如果是敌对才显示
        if (!isFriendly)
            EnemylyIcon.SetActive(set);
    }
}
