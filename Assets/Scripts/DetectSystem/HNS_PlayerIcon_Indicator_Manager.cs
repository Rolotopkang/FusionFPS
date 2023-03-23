using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HNS_PlayerIcon_Indicator_Manager : MonoBehaviour
{
    [SerializeField] private Image Image;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private GameObject HPBar;
    [SerializeField] private Text dis;
    [SerializeField] private Image fill;
    [SerializeField] private Image fillB;

    [SerializeField] private Color EColor;
    [SerializeField] private Color FColor;

    private bool IsGazed = false;
    private bool IsEnemy = false;
    
    public void Initialisation(bool _isEnemy , string _name)
    {
        //指示器颜色初始化
        Image.color = _isEnemy? Color.red : Color.cyan;
        dis.color = _isEnemy? Color.red : Color.cyan;
        name.color = _isEnemy? Color.red : Color.cyan;
        fill.color = _isEnemy? Color.red : Color.cyan;
        fillB.color = _isEnemy? EColor : FColor;
        
        //UI初始化
        HPBar.SetActive(false);
        name.text = _name;
        IsEnemy = _isEnemy;
        Image.gameObject.SetActive(!_isEnemy);
        name.gameObject.SetActive(false);
        dis.transform.parent.gameObject.SetActive(false);
    }

    public void UpdateHPInfo(float _fill)
    {
        //除非被瞄准才显示血条相关
        if (IsGazed)
        {
            if (_fill == 1)
            {
                HPBar.SetActive(false);
            }
            else
            {
                HPBar.SetActive(true);
            }
            fill.fillAmount = _fill;
        }
        else
        {
            HPBar.SetActive(false);
        }
    }

    public void SetGazed(bool _isActive)
    {
        IsGazed = _isActive;
        name.gameObject.SetActive(IsGazed);
        dis.transform.parent.gameObject.SetActive(IsGazed);
        if (IsEnemy)
        {
            Image.gameObject.SetActive(_isActive);
        }
    }

    public void SetArounded(bool _isActive)
    {
        if(IsGazed)
            return;
        if (IsEnemy)
        {
            Image.gameObject.SetActive(_isActive);
        }
    }

}
