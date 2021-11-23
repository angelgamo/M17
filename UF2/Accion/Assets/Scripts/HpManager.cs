using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpManager : MonoBehaviour
{
    [Header("Healt")]
    [SerializeField] int maxHp;
    int currentHp;

    [Header("Invencible Frames")]
    [SerializeField] float ivTime;
    bool IVF;

    [Header("Health Bar Variables")]
    [SerializeField] Vector3 healtbarOffset;
    [SerializeField] Vector2 healtbarSize;
    [SerializeField] float healtbarBorderSize;
    [SerializeField] Color healtbarColor;
    [SerializeField] Color damagebarColor;
    [SerializeField] Sprite healthBar;
    [SerializeField] float healtbarDamage;
    [SerializeField] float healtbarDamage2;
    Transform scaleHealthTransform;
    Transform scaleDamageTransform;
    Vector3 scaleHealth;
    Vector3 scaleDamage;
    GameObject healhtBar;
    GameObject health;

    // Shield
    public bool onShield;
    public bool flip;

    public delegate void Death();
    public Death onDeath;

    private void Awake()
    {
        healhtBar = new GameObject();
        health = new GameObject();
        GameObject damage = new GameObject();
        GameObject bar = new GameObject();

        healhtBar.name = "HealthBar";
        health.name = "Health";
        damage.name = "Damage";
        bar.name = "Bar";

        healhtBar.transform.parent = this.transform;
        bar.transform.parent = healhtBar.transform;
        damage.transform.parent = healhtBar.transform;
        health.transform.parent = healhtBar.transform;

        healhtBar.transform.position = this.transform.position + healtbarOffset;
        health.transform.position += new Vector3(healtbarBorderSize / 2, 0);
        damage.transform.position += new Vector3(healtbarBorderSize / 2, 0);

        health.AddComponent<SpriteRenderer>();
        damage.AddComponent<SpriteRenderer>();
        bar.AddComponent<SpriteRenderer>();

        health.GetComponent<SpriteRenderer>().sprite = healthBar;
        damage.GetComponent<SpriteRenderer>().sprite = healthBar;
        bar.GetComponent<SpriteRenderer>().sprite = healthBar;

        health.GetComponent<SpriteRenderer>().color = healtbarColor;
        damage.GetComponent<SpriteRenderer>().color = damagebarColor;
        bar.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, .8f);

        health.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
        damage.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
        bar.GetComponent<SpriteRenderer>().sortingLayerName = "UI";

        health.transform.localScale = new Vector3(healtbarSize.x, healtbarSize.y);
        damage.transform.localScale = new Vector3(healtbarSize.x, healtbarSize.y);
        bar.transform.localScale = new Vector3(healtbarSize.x + healtbarBorderSize, healtbarSize.y + healtbarBorderSize);

        scaleHealthTransform = health.transform;
        scaleDamageTransform = damage.transform;
        scaleHealth = scaleHealthTransform.localScale;
        scaleDamage = scaleDamageTransform.localScale;

        health.GetComponent<SpriteRenderer>().sortingOrder = -1;
        damage.GetComponent<SpriteRenderer>().sortingOrder = -2;
        bar.GetComponent<SpriteRenderer>().sortingOrder = -3;

    }
    void Start()
    {
        IVF = false;
        currentHp = maxHp;
    }

    public void RecieveDamage2(int damage)
    {
        if (currentHp <= 0 || IVF) return;
        StartCoroutine(IVFrames());

        currentHp -= damage;
        SetHealtBar(Mathf.Clamp01((float)currentHp / maxHp) * healtbarSize.x);
        StopCoroutine("HealtBarDamaged");
        StartCoroutine("HealtBarDamaged");

        if (currentHp <= 0)
        {
            currentHp = 0;
            if (onDeath != null) onDeath.Invoke();
            this.gameObject.SetActive(false);
        }
    }

    void SetHealtBar(float health)
    {
        scaleHealth.x = health;
        scaleHealthTransform.localScale = scaleHealth;
    }

    IEnumerator HealtBarDamaged()
    {
        yield return new WaitForSeconds(healtbarDamage);

        float time = 0;
        Vector3 startPosition = scaleDamage;

        while (time < healtbarDamage2)
        {
            scaleDamage = Vector3.Lerp(startPosition, scaleHealth, time / healtbarDamage2);
            scaleDamageTransform.localScale = scaleDamage;
            time += Time.deltaTime;
            yield return null;
        }
        scaleDamage = scaleHealth;
    }

    IEnumerator IVFrames()
    {
        IVF = true;
        yield return new WaitForSecondsRealtime(ivTime);
        IVF = false;
    }

    void heal(int heal) {
        currentHp += heal;
        if (currentHp > maxHp) 
        {
            currentHp = maxHp;
        }
    }

    public void ShieldRight()
    {
        onShield = true;
        flip = false;
    }

    public void ShieldLeft()
    {
        onShield = true;
        flip = true;
    }

    public void NoShield()
    {
        onShield = false;
    }
}
