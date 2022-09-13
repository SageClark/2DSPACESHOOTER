using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{    
    private Image _barImage;    
    private Energy _energy;

    void Start()
    {
        _energy = new Energy();
        _barImage = GetComponent<Image>();
    }

    void Update()
    {
        _barImage.fillAmount = _energy.GetEnergyNormalized();
        _energy.Update();
    }

    public class Energy
    {
        public const int ENERGY_MAX = 100;

        private float energyAmount;
        private float energyRegenAmount;
        private float _timeToWaitToRegen = 1f + Time.time;

        public Energy()
        {
            energyAmount = 0;
            energyRegenAmount = 30f;
        }

        public void Update()
        {
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.LeftShift) && _timeToWaitToRegen < Time.time && energyAmount < ENERGY_MAX)
            {
                energyAmount += energyRegenAmount * Time.deltaTime;
            }
            
            if(Input.GetKeyUp(KeyCode.LeftShift) && _timeToWaitToRegen < Time.time && energyAmount < ENERGY_MAX)
            {
                energyAmount += energyRegenAmount * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                TrySpendEnergy(0.6f);
                _timeToWaitToRegen = 1f + Time.time;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                TrySpendEnergy(0.6f);
                _timeToWaitToRegen = 1f + Time.time;
            }
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
