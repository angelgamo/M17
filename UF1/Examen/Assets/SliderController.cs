using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SliderController : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        PlayerPrefs.SetInt("color", 0);
        PlayerPrefs.SetInt("keyboardmouse", 0);
    }

    public void ChangeValue()
    {
        PlayerPrefs.SetInt("color", (int)slider.value);
    }

    public void ChangeScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void KeyboardMouse(bool i)
    {
        if (i) PlayerPrefs.SetInt("keyboardmouse", 0);
        else PlayerPrefs.SetInt("keyboardmouse", 1);
    }
}
