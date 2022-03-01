using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Weapon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScopeUI : MonoBehaviour
{
    public Firearms Firearms;
    private TextMeshProUGUI Text;
    private Color nowColor;
    public Color NormalColor;
    [Range(0,5)]
    public float intensity;

    public int YellowNum = 11;
    public int RedNum = 6;

    private void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        int tmp_CurrentAmmo = Firearms.GetCurrentAmmo;
        Text.text = tmp_CurrentAmmo.ToString();
        if (tmp_CurrentAmmo < YellowNum && tmp_CurrentAmmo >= RedNum)
        {
            ColorUtility.TryParseHtmlString("#FF8C00", out nowColor);
        }else if (tmp_CurrentAmmo < RedNum)
        {
            ColorUtility.TryParseHtmlString("#FF001D", out nowColor);
        }
        else
        {
            nowColor = NormalColor;
        }
        Text.color = nowColor* intensity;
    }
}
