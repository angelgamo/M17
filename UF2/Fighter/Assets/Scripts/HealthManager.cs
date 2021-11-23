using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Health Points Variables")]
    [SerializeField] float maxHp;
    float currentHp;

    [Header("Collisions Variables")]
    [SerializeField] float ivFrames;
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
    int layer;
    public bool isDead;

    [Header("PopUp Text")]
    [SerializeField] GameObject popUp;

    [Header("Extra")]
    [SerializeField] ParticleSystem blood;
    [SerializeField] GameObject body;
    [SerializeField] GameObject ragdoll;
    GameObject ragdollClone;

    public delegate void Death();
    public Death onDeath;

    public delegate void Health(float health);
    public Health onHealth;

    public delegate void Hit(float damage);
    public Hit onHit;

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
        health.transform.position += new Vector3(healtbarBorderSize/2, 0);
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
        layer = gameObject.layer;
        isDead = false;
    }

    public void RecieveDamage(float damage, bool mana = true)
    {
        if (currentHp <= 0 || IVF) return;
        StartCoroutine(IVFrames());

        blood.Play();
        PopUp(damage.ToString());
        currentHp -= damage;
        if (onHit != null && mana) onHit.Invoke(damage);
        SetHealtBar(Mathf.Clamp01(currentHp / maxHp) * healtbarSize.x);
        StopCoroutine("HealtBarDamaged");
        StartCoroutine("HealtBarDamaged");

        if (currentHp <= 0)
        {
            isDead = true;
            this.gameObject.layer = 9;
            body.SetActive(false);
            healhtBar.SetActive(false);
            ragdollClone = Instantiate(ragdoll, transform.position, Quaternion.identity);
            if (onDeath != null) onDeath.Invoke();
            currentHp = 0;
        }
    }

    public void Heal()
    {
        currentHp = maxHp;
        this.gameObject.layer = layer;
        if (ragdollClone != null) Destroy(ragdollClone);
        isDead = false;
        body.SetActive(true);
        healhtBar.SetActive(true);
        SetHealtBar(healtbarSize.x);
        SetDamageBar();
    }

    void SetHealtBar(float health)
    {
        scaleHealth.x = health;
        scaleHealthTransform.localScale = scaleHealth;
        if (onHealth != null) onHealth.Invoke(health / healtbarSize.x);
    }

    void SetDamageBar()
    {
        scaleDamageTransform.localScale = scaleHealth;
    }

    public float getMaxHP()
    {
        return maxHp;
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
        health.GetComponent<SpriteRenderer>().color = Color.cyan;

        yield return new WaitForSecondsRealtime(ivFrames);

        IVF = false;
        health.GetComponent<SpriteRenderer>().color = healtbarColor;
    }

    void PopUp(string text)
    {
        GameObject popUpClone = Instantiate(popUp, transform.position + transform.up, Quaternion.identity);
        popUpClone.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().SetText("-" + text);
    }
}