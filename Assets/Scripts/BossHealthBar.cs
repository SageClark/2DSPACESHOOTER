using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private Image _barImage;
    private float _bossHealth;
    private Boss _boss;

    void Start()
    {
        _barImage = GetComponent<Image>();
        if (_barImage == null)
        {
            Debug.LogError("Bar Image is null (enemy)");
        }

        _boss = GameObject.Find("Boss").GetComponent<Boss>();
        if (_boss == null)
        {
            Debug.LogError("Boss is null (enemy)");
        }
    }

    void Update()
    {
        _bossHealth = _boss.GetBossHealth();
        _barImage.fillAmount = _bossHealth;
    }
}