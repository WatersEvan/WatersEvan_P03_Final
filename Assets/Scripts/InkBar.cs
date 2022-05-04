using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InkBar : MonoBehaviour
{
    
    public Slider slider;

    public void SetInk(int ink)
    {
        slider.value = ink;
    }
}
