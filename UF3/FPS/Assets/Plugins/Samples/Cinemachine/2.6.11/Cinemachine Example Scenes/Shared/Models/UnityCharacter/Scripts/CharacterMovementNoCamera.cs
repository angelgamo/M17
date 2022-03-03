using UnityEngine;

// WASD to move, Space to sprint
public class CharacterMovementNoCamera : MonoBehaviour
{
    public new Transform camera;

    public float StrafeSpeed = 0.1f;
    public float TurnSpeed = 3;
    public float Damping = 0.2f;
    public float VerticalRotMin = -80;
    public float VerticalRotMax = 80;
    public KeyCode sprintJoystick = KeyCode.JoystickButton2;
    public KeyCode sprintKeyboard = KeyCode.Space;

    private bool isSprinting;
    private Animator anim;
    private float currentStrafeSpeed;
    private Vector2 currentVelocity;

    void OnEnable()
    {
        camera = Camera.main.transform;
        anim = GetComponent<Animator>();
        currentVelocity = Vector2.zero;
        currentStrafeSpeed = 0;
        isSprinting = false;
    }

    void FixedUpdate()
    {
        var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        var speed = input.y;
        speed = Mathf.Clamp(speed, -1f, 1f);
        speed = Mathf.SmoothDamp(anim.GetFloat("Speed"), speed, ref currentVelocity.y, Damping);
        anim.SetFloat("Speed", speed);
        anim.SetFloat("Direction", speed);

        // set sprinting
        isSprinting = (Input.GetKey(sprintJoystick) || Input.GetKey(sprintKeyboard)) && speed > 0;
        anim.SetBool("isSprinting", isSprinting);

        // strafing
        currentStrafeSpeed = Mathf.SmoothDamp(
            currentStrafeSpeed, input.x * StrafeSpeed, ref currentVelocity.x, Damping);
        transform.position += transform.TransformDirection(Vector3.right) * currentStrafeSpeed;

        //var rotInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //var rot = transform.eulerAngles;
        //rot.y += rotInput.x * TurnSpeed;
        var rot = new Vector3(0, camera.transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(rot);
    }
}
