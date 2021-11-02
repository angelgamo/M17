using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUIManager : MonoBehaviour
{
    Player player;

    public TMPro.TextMeshProUGUI logs;
    public TMPro.TextMeshProUGUI food;
    public TMPro.TextMeshProUGUI gold;
    public TMPro.TextMeshProUGUI rocks;
    public TMPro.TextMeshProUGUI villagers;

    int a = 0;

    private void Start()
    {
        this.player = (Player)GameObject.Find("Player").GetComponent<PlayerManager>().player;
        StartCoroutine(UpdateValues());
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            player.mouseOverUI = true;
        }
        else
        {
            player.mouseOverUI = false;
        }
    }


    IEnumerator UpdateValues()
    {
        while (true)
        {
            logs.text = player.logsCount.ToString();
            food.text = player.foodCount.ToString();
            gold.text = player.goldCount.ToString();
            rocks.text = player.rocksCount.ToString();
            villagers.text = player.villagersCount.ToString() + "/" + player.villagersCapacityCount.ToString();
            yield return new WaitForSeconds(.2f);
        }
    }

    private void OnMouseOver()
    {
        print(a);
        a++;
    }
}
