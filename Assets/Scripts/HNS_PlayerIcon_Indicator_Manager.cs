using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HNS_PlayerIcon_Indicator_Manager : MonoBehaviour
{
    [SerializeField]
    private Image Image;
    [SerializeField]
    private TextMeshProUGUI name;

    [SerializeField] private Text dis;

    public void Initialisation(bool isEnemy , string _name)
    {
        //指示器颜色初始化
        Image.color = isEnemy? Color.red : Color.cyan;
        dis.color = isEnemy? Color.red : Color.cyan;
        name.color =isEnemy? Color.red : Color.cyan;
        
        name.text = _name;
    }

    public void UpdateInfo(int _dis)
    {
        dis.text = _dis.ToString();
    }

    public void SetActiveName(bool _isActive)
    {
        name.gameObject.SetActive(_isActive);
    }
}
