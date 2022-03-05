using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    public PlayerNumericalController PlayerNumericalController;
    public Image HealthBar;
    public TextMeshProUGUI CurrHealth;
    public TextMeshProUGUI MaxHealth;
    private void Update()
    {
        UpdateHealthBarUI();
    }
    
    private void UpdateHealthBarUI()
    {
        HealthBar.fillAmount =
            (float)PlayerNumericalController.Health /PlayerNumericalController.MaxHealth;
        MaxHealth.text = PlayerNumericalController.MaxHealth.ToString();
        CurrHealth.text = PlayerNumericalController.Health.ToString();
    }
}
