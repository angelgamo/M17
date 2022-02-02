using UnityEngine;

public class State2Rotate : State2
{
    [Header("Settings")]
    [SerializeField] float rotationSpeed;

    public override void Init()
    {
        
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    public override void Exit()
    {

    }
}
