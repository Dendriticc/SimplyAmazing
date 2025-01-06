using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    [SerializeField]
    Slider StaminaBar;
    [Range(0, 100f)]public static float stamina;
    [Range(-1f, 3f)] static float fatiguePeriod;

    void Awake()
    {
        stamina = 100f;
    }

    void Update()
    {
        if (stamina > 100) stamina = 100;
        StaminaBar.value = stamina;
    }
}
