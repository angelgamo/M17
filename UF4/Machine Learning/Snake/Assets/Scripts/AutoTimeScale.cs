using UnityEngine;

public class AutoTimeScale : MonoBehaviour
{
    float deltaTime;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        if (fps > 50.0f)
            Time.timeScale += 0.01f;
        else if (fps < 48f)
            Time.timeScale -= 0.01f;
    }
}
