using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildingController : MonoBehaviour
{
    public GameObject builder;
    public GameObject units;
    public GameObject resources;
    public GameObject[] others;
    public delegate void Delegao();
    public event Delegao cursorBasic;
    public event Delegao cursorHand;
    public event Delegao cursorBuild;
    public event Delegao cursorAttack;
    public event Delegao cursorMov;
    public event Delegao mantenerCursor;
    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            builder.gameObject.SetActive(!builder.gameObject.activeSelf);
            for(int i = 0; i < others.Length; i++)
            {
                others[i].SetActive(false);
            }
        }
        if (Input.GetKeyDown("u"))
        {
            units.gameObject.SetActive(!units.gameObject.activeSelf);
            for (int i = 0; i < others.Length; i++)
            {
                others[i].SetActive(false);
            }
        }
        if (Input.GetKeyDown("r"))
        {
            resources.gameObject.SetActive(!resources.gameObject.activeSelf);
            for (int i = 0; i < others.Length; i++)
            {
                others[i].SetActive(false);
            }
        }
    }

    public void ChangeActive(int indexEvent)
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);

        switch (indexEvent)
        {
            case 0: cursorBasic.Invoke();
                break;
            case 1:
                cursorHand.Invoke();
                break;
            case 2:
                cursorBuild.Invoke();
                break;
            case 3:
                cursorAttack.Invoke();
                break;
            case 4:
                cursorMov.Invoke();
                break;
            default: mantenerCursor.Invoke();
                break;
        }

    }

    public void SetFalse()
    {
        this.gameObject.SetActive(false);
    }
}
