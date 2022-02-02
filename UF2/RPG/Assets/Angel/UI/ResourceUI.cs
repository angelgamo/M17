using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI ValueText;
    public Image FronBar;
    public Image BackBar;
    public float lerpTime;
    public float delayTime;

    public void ResetValue()
    {
        StopCoroutine("LerpBar");
        BackBar.fillAmount = 0;
        FronBar.fillAmount = 0;
    }

    public void ModifyValue(float value)
    {
        FronBar.fillAmount = value;
        StopCoroutine("LerpBar");
        StartCoroutine("LerpBar");
    }

    public void ModifyValue(float value, float value1, float value2)
    {
        ModifyValue(value);
        ValueText.text = (int)value1 + "/" + (int)value2;
    }

    IEnumerator LerpBar()
    {
        yield return new WaitForSeconds(delayTime);
        float time = 0;
        float startValue = BackBar.fillAmount;

        while (time < lerpTime)
        {
            time += Time.deltaTime;
            BackBar.fillAmount = startValue + (FronBar.fillAmount - startValue) * time / lerpTime;
            yield return null;
        }
        BackBar.fillAmount = FronBar.fillAmount;
    }
}
