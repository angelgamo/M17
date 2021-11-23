using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] HealthManager player1HealthManager;
    [SerializeField] HealthManager player2HealthManager;
    [SerializeField] PlayerController player1PlayerController;
    [SerializeField] PlayerController player2PlayerController;
    int player1Lives;
    int player2Lives;
    int player1Score;
    int player2Score;
    bool someoneDied;

    [Header("UI")]
    [SerializeField] Image player1HealtBar;
    [SerializeField] Image player1ManaBar;
    [SerializeField] GameObject player1Live1;
    [SerializeField] GameObject player1Live2;
    [SerializeField] Image player2HealtBar;
    [SerializeField] Image player2ManaBar;
    [SerializeField] GameObject player2Live1;
    [SerializeField] GameObject player2Live2;
    [SerializeField] GameObject puntuacionPopUp;

    private void Start()
    {
        player1Lives = 2;
        player2Lives = 2;
        player1Score = 0;
        player2Score = 0;

        player1HealthManager.onDeath += LossLivePlayer1;
        player2HealthManager.onDeath += LossLivePlayer2;

        player1HealthManager.onHealth += UpdateHealthPlayer1;
        player2HealthManager.onHealth += UpdateHealthPlayer2;

        player1PlayerController.onMana += UpdateManaPlayer1;
        player2PlayerController.onMana += UpdateManaPlayer2;

        someoneDied = false;
    }

    void Restart()
    {
        PuntuacionPopUp();

        
        

        StartCoroutine(RespawnPlayer1B());
        StartCoroutine(RespawnPlayer2B());

        player1Live1.SetActive(true);
        player1Live2.SetActive(true);
        player2Live1.SetActive(true);
        player2Live2.SetActive(true);
    }

    void LossLivePlayer1()
    {
        if (!someoneDied) player1Lives--;
        someoneDied = true;

        if (player1Lives <= 0)
        {
            player2Score++;
            player1Live1.SetActive(false);
            player1Live2.SetActive(false);
            Restart();
        }
        else if (player1Lives == 1)
        {
            player1Live2.SetActive(false);
            StartCoroutine(RespawnPlayer1A());
        }
        else
        {
            StartCoroutine(RespawnPlayer1A());
        }
    }

    void LossLivePlayer2()
    {
        if (!someoneDied) player2Lives--;
        someoneDied = true;

        if (player2Lives <= 0)
        {
            player1Score++;
            player2Live1.SetActive(false);
            player2Live2.SetActive(false);
            Restart();
        }
        else if (player2Lives == 1)
        {
            player2Live2.SetActive(false);
            StartCoroutine(RespawnPlayer2A());
        }
        else
        {
            StartCoroutine(RespawnPlayer2A());
        }
    }

    IEnumerator RespawnPlayer1A()
    {
        yield return new WaitForSeconds(3f);
        someoneDied = false;
        player1HealthManager.Heal();
        player1HealthManager.transform.position = Vector3.zero;
    }

    IEnumerator RespawnPlayer1B()
    {
        yield return new WaitForSeconds(3f);
        someoneDied = false;
        player1Lives = 2;
        player1PlayerController.ResetMana();
        player1HealthManager.Heal();
        player1HealthManager.transform.position = new Vector3(-5, -2.5f);
    }

    IEnumerator RespawnPlayer2A()
    {
        yield return new WaitForSeconds(3f);
        someoneDied = false;
        player2HealthManager.Heal();
        player2HealthManager.transform.position = Vector3.zero;
    }

    IEnumerator RespawnPlayer2B()
    {
        yield return new WaitForSeconds(3f);
        someoneDied = false;
        player2Lives = 2;
        player2PlayerController.ResetMana();
        player2HealthManager.Heal();
        player2HealthManager.transform.position = new Vector3(4, 4.5f);
    }

    void PuntuacionPopUp()
    {
        puntuacionPopUp.GetComponent<TMPro.TextMeshProUGUI>().SetText(player1Score + "-" + player2Score);
        puntuacionPopUp.SetActive(true);
    }

    void UpdateHealthPlayer1(float health)
    {
        player1HealtBar.fillAmount = health;
    }

    void UpdateHealthPlayer2(float health)
    {
        player2HealtBar.fillAmount = health;
    }

    void UpdateManaPlayer1(float mana)
    {
        player1ManaBar.fillAmount = mana;
    }

    void UpdateManaPlayer2(float mana)
    {
        player2ManaBar.fillAmount = mana;
    }
}
