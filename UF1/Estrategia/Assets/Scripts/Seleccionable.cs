using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seleccionable : MonoBehaviour
{
    private SpriteRenderer sprRenderer;
    private Vector3 mouse;
    public bool selected;
    private Selector2 sel2=null;

    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        selected = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Seleccionador")
        {
            sprRenderer.color = Color.green;
            selected = true;
            if (sel2 == null)  sel2 = collision.GetComponent<Selector2>();
            sel2.onSetTarget += Activar;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Selector2>() && Input.GetMouseButton(0))
        {
            UnSuscribe();
        }
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UnSuscribe();
        }
        if (Input.GetMouseButtonDown(1) && selected)
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = transform.position.z;
        }
    }

    void UnSuscribe()
    {
        sprRenderer.color = Color.white;
        selected = false;
        try
        {
            sel2.onSetTarget -= Activar;
        }
        catch { }
    }

    private void OnDestroy()
    {
        UnSuscribe();
    }

    void Activar(Transform target)
    {
        GetComponent<AI>().enabled = false;
        transform.GetChild(1).GetComponent<Attack>().enabled = false;
        transform.GetChild(2).GetComponent<Vision>().enabled = false;
        GetComponent<Pathfinding.AIDestinationSetter>().enabled = true;
        GetComponent<Pathfinding.AILerp>().enabled = true;
        GetComponent<Pathfinding.Seeker>().enabled = true;
        GetComponent<Pathfinding.AIDestinationSetter>().target = target;
    }
}
