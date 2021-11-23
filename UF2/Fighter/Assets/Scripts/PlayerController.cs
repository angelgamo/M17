using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // Movement Controller
    Rigidbody2D rb;

    [Header("Movement Keybinds")]
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode up;
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode attack1;
    [SerializeField] KeyCode attack2;
    [SerializeField] KeyCode attack3;

    [Header("Movement Variables")]
    [SerializeField] float _jumpForce;
    [SerializeField] float _accMoveSpeed;
    [SerializeField] float _maxMoveSpeed;
    [SerializeField] ParticleSystem _dust;

    float movementX;
    bool jump;
    bool move;
    bool isGrounded;
    bool _changingDirection => (rb.velocity.x > 0f && movementX < 0f || rb.velocity.x < 0f && movementX > 0f);

    [Header("Mana Variables")]
    [SerializeField] HealthManager enemy;
    [SerializeField] int _maxMana;
    [SerializeField] int _manaCost;
    int _mana;
    [SerializeField] Vector2 grenadeVelocity;
    public delegate void Mana(float mana);
    public Mana onMana;

    // Srite Controller
    Animator anim;
    bool flip;

    // Combo Controller
    [Header("Combo Variables")]
    [SerializeField] float tempsMin = 0.1f;
    [SerializeField] float tempsMax = 1f;
    string state = "-";
    bool tooSoon = false;

    [Header("Hitbox Variables")]
    [SerializeField] PlayerSO playerSO;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        enemy.onHit += AddMana;
        _mana = 0;
    }

    void Update()
    {
        // Movement Controller
            movementX = Input.GetKey(left) ? -1 : Input.GetKey(right) ? 1: 0;
            move = movementX == 0 ? false : true;
            jump =  Input.GetKeyDown(up) && isGrounded ? jump = true : jump;

            // OnGround
            Debug.DrawRay(transform.position + Vector3.down, Vector3.down * 0.1f, Color.red);
            if (Physics2D.Raycast(transform.position + Vector3.down, Vector3.down, 0.1f))
            {
            if (!isGrounded) _dust.Play();
                isGrounded = true;
            }
            else isGrounded = false;

        // Sprite Controller
            anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
            anim.SetBool("isGrounded", isGrounded);

        // Combo Controller
            if (Input.GetKeyDown(attack1)) StartCoroutine(comboNewInput("A"));
            else if (Input.GetKeyDown(attack2)) StartCoroutine(comboNewInput("B"));
            else if (Input.GetKeyDown(attack3)) StartCoroutine(comboNewInput("C"));
    }

    private void FixedUpdate()
    {
        // Movement Controller

            if (move && Mathf.Abs(rb.velocity.x) < _maxMoveSpeed || move && _changingDirection) rb.AddForce(Vector2.right * _accMoveSpeed * movementX);
            if (isGrounded) rb.velocity += Vector2.right * -(Mathf.Clamp(rb.velocity.x, -.5f, .5f));
            if (jump)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * _jumpForce * 10 * Time.deltaTime);
                jump = false;
                _dust.Play();
            }

        // Sprite Controller
            flip = Input.GetKey(left) ? true : Input.GetKey(right) ? false : rb.velocity.x < -0.01f ? true : rb.velocity.x > 0.01f ? false : flip;
            anim.SetFloat("flip", flip ? 0 : 1);
    }

    IEnumerator comboNewInput(string input)
    {
        if (!tooSoon)
        {
            switch (state)
            {
                case "-":
                    if (input == "A")           // combo A - AttackA - 10 damage
                    {
                        anim.SetTrigger("A");
                        state = "A";
                    }
                    else if (input == "B")      // combo B - AttackB - 10 damage
                    {
                        anim.SetTrigger("B");
                        state = "B";
                    }
                    else if (input == "C")      // combo C - AttackGranada - 40 damage
                    {
                        if (_mana >= _manaCost)
                        {
                            anim.SetTrigger("C");
                            _mana -= _manaCost;
                            onMana.Invoke((float)_mana / _maxMana);
                        }
                        state = "-";
                    }
                    break;
                case "A":
                    if (input == "A")           // combo AA - AttackC - 20 damage
                    {
                        anim.SetTrigger("AA");
                        state = "-";
                    }
                    else if (input == "B")      // combo AB - AttackD - 20 damage
                    {
                        anim.SetTrigger("AB");
                        state = "AB";
                    }
                    else if (input == "C")      // combo AC [null]
                    {
                        CrearTriggers();
                        state = "-";
                    }
                    break;
                case "B":
                    if (input == "A")           // combo BA [null]
                    {
                        CrearTriggers();
                        state = "-";
                    }
                    else if (input == "B")      // combo BB - AttackF - 20 damage
                    {
                        anim.SetTrigger("BB");
                        state = "BB";
                    }
                    else if (input == "C")      // combo BC [null]
                    {
                        CrearTriggers();
                        state = "-";
                    }
                    break;
                case "AB":
                    if (input == "A")           // combo ABA - AttackE - 30 damage
                    {
                        anim.SetTrigger("ABA");
                        state = "-";
                    }
                    else if (input == "B")      // combo ABB [null]
                    {
                        CrearTriggers();
                        state = "-";
                    }
                    else if (input == "C")      // combo ABC [null]
                    {
                        CrearTriggers();
                        state = "-";
                    }
                    break;
                case "BB":
                    if (input == "A")           // combo BBA - AttackG - 30 damage
                    {
                        anim.SetTrigger("BBA");
                        state = "-";
                    }
                    else if (input == "B")      // combo BBB [null]
                    {
                        CrearTriggers();
                        state = "-";
                    }
                    else if (input == "C")      // combo BBC [null]
                    {
                        CrearTriggers();
                        state = "-";
                    }
                    break;
            }
        }



        string copystate = state;
        tooSoon = true;
        yield return new WaitForSeconds(tempsMin);
        tooSoon = false;
        yield return new WaitForSeconds(tempsMax - tempsMin);
        if (state == copystate && state != "-")
        {
            state = "-";
            CrearTriggers();
        }

    }

    void CrearTriggers()
    {
        anim.ResetTrigger("A");
        anim.ResetTrigger("AA");
        anim.ResetTrigger("AB");
        anim.ResetTrigger("ABA");
        anim.ResetTrigger("B");
        anim.ResetTrigger("BB");
        anim.ResetTrigger("BBA");
        anim.ResetTrigger("C");
    }

    public void SetAttackDamage(int damage)
    {
        playerSO.damage = damage;
    }

    public void AddMana(float damage)
    {
        this._mana += (int)(damage * damage)/2;
        if (this._mana > _maxMana) this._mana = _maxMana;
        onMana.Invoke((float)_mana / _maxMana);
    }

    public void ResetMana()
    {
        _mana = 0;
        onMana.Invoke(0);
    }

    public void ThrowGrenadeRight()
    {
        GameObject grenade = GetComponent<ObjectPool>().GetPooledObject();
        if (grenade == null) return;
        grenade.transform.position = this.transform.position;
        grenade.gameObject.SetActive(true);
        grenade.GetComponent<Rigidbody2D>().AddForce(grenadeVelocity, ForceMode2D.Impulse);
    }

    public void ThrowGrenadeLeft()
    {
        GameObject grenade = GetComponent<ObjectPool>().GetPooledObject();
        if (grenade == null) return;
        grenade.transform.position = this.transform.position;
        grenade.gameObject.SetActive(true);
        grenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(-grenadeVelocity.x, grenadeVelocity.y), ForceMode2D.Impulse);
    }
}
