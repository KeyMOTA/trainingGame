using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public GameObject headLight;
    private bool isLightOn = false;

    void Update()
    {
        if (Input.GetButtonDown("Light"))
        {
            isLightOn = !isLightOn;
            headLight.SetActive(isLightOn);
        }
    }
}
