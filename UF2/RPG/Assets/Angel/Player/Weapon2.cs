using UnityEngine;
using UnityEngine.EventSystems;

public class Weapon2 : MonoBehaviour
{
    Animator anim;
    Transform playerTransform;
    float angle;
    SpriteRenderer weaponImage;

    [HideInInspector] public bool rotate = true;

	private void Awake()
	{
        anim = transform.GetChild(0).GetComponent<Animator>();
        playerTransform = transform.parent;
        weaponImage = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        angle = GetAngle(playerTransform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (rotate)
            WeaponRotation();

        if (Input.GetMouseButton(0)) 
            if (weaponImage.sprite != null)
                anim.SetTrigger("Attack");
    }
    
    void WeaponRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    float GetAngle(Vector2 point1, Vector2 point2)
    {
        Vector2 delta = point1 - point2;
        return ((Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg) + 180);
    }

    public void ChangeAttackSpeed(float speed)
	{
        anim.SetFloat("Speed", speed);
	}
}
