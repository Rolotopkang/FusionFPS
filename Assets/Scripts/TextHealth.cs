using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextHealth : Element
{
    #region SerializeField

    [SerializeField]
    private TextMeshProUGUI textHealth;

    [SerializeField]
    private Image healthBar;
    
    #endregion
    
    #region private

    private float maxHealth;
    private float currHealth;
    

    #endregion
    
    
    #region Unity

    protected override void Start()
    {
        base.Start();
        maxHealth = Battle.GetMaxHealth();
        currHealth = Battle.GetCurrentHealth();
    }

    protected override void Tick()
    {
        float tmp_health = Battle.GetCurrentHealth();
        textHealth.text = ((int)tmp_health).ToString();
        healthBar.fillAmount = tmp_health / maxHealth;
    }

    #endregion
}
