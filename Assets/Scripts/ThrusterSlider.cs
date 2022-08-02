using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterSlider : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            slider.value--;
        }
        if (slider.value < slider.maxValue)
        {
            StartCoroutine(sliderRefillRoutine());
        }

    }

    IEnumerator sliderRefillRoutine()
    {
        while (slider.value < slider.maxValue)
        {
            
            yield return new WaitForSeconds(3);
            slider.value++;
            break;
        }

    }
}
