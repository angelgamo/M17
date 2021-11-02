using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeCursor : MonoBehaviour
{
    public Texture2D[] cursores;
    Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.ForceSoftware;
    private Vector2 hotSpot = Vector2.zero;

    public UIBuildingController uiC;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
        cursorTexture = cursores[0];
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

        /*
        uiC.cursorBasic += CursorBasic;
        uiC.cursorHand += CursorMa;
        uiC.cursorBuild += CursorBuild;
        uiC.cursorAttack += CursorAttack;
        uiC.cursorMov += CursorMov;
        uiC.mantenerCursor += MantenerCursor;
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
   /* void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void OnMouseExit()
    {
        
    }

    void CursorBasic()
    {
        cursorTexture = cursores[0];
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void CursorMa()
    {
        cursorTexture = cursores[1];
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void CursorBuild()
    {
        cursorTexture = cursores[2];
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void CursorAttack()
    {
        cursorTexture = cursores[3];
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void CursorMov()
    {
        cursorTexture = cursores[4];
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void MantenerCursor()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
   */
}
