using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    Transform player;

    [Header("Interact Settings")]
    [SerializeField, Range(.1f, 5f)] float radius = 1f;
    [SerializeField, Range(.05f, .5f)] float checkTime = .1f;
    [SerializeField] KeyCode interactKey = KeyCode.E;
    [SerializeField] bool autoStart = true;
    [SerializeField] bool autoInteract;
    [SerializeField] UnityEvent interactCallBack;

    [Space]
    [SerializeField] bool debug;
    [HideInInspector] public bool isInteractable;
    protected bool hasInteracted;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (autoStart)
            StartCoroutine("Check");
    }

    private void Update()
    {
        if (autoInteract || hasInteracted) return;

        if (Input.GetKey(interactKey))
            isInteractable = true;
        else
            isInteractable = false;
    }

    public IEnumerator Check()
    {
        while (!hasInteracted)
        {
            float distance = Vector2.Distance(player.position, transform.position);
            if (distance <= radius)
            {
                if (autoInteract || isInteractable)
                    Interact();
            }
            yield return new WaitForSeconds(checkTime);
        }
    }

    protected virtual void Interact()
    {
        interactCallBack.Invoke();
        hasInteracted = true;
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void Restart()
    {
        hasInteracted = false;
        if (autoStart)
            StartCoroutine("Check");
    }
}