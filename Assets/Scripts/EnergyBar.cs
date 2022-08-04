using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{    
    private Image barImage;    
    private Energy _energy;

    void Start()
    {
        _energy = new Energy();
        barImage = GetComponent<Image>();
    }

    void Update()
    {
        _energy.Update();

        barImage.fillAmount = _energy.GetEnergyNormalized();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _energy.TrySpendEnergy(1.2f);
        }
    }

    public class Energy
    {
        public const int ENERGY_MAX = 100;

        private float energyAmount;
        private float energyRegenAmount;

        public Energy()
        {
            energyAmount = 0;
            energyRegenAmount = 30f;
        }

        public void Update()
        {
            energyAmount += energyRegenAmount * Time.deltaTime;
        }

        public void TrySpendEnergy(float amount)
        {
            if(energyAmount >= amount)
            {
                energyAmount -= amount;
            }
        }

        public float GetEnergyNormalized()
        {
            return energyAmount / ENERGY_MAX;
        }
    }
}
