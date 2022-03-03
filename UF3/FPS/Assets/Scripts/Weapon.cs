using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
	public UnityEvent reload;

	class Bullet
	{
		public float time;
		public Vector3 position;
		public Vector3 velocity;
		public TrailRenderer tracerEffect;
	}

	[Header("Presets")]
	[Expandable] public PhysicsSystem worldPhysics;
	[Expandable] public WeaponPreset weaponPreset;

	[Header("Effects")]
	public TrailRenderer tracerEffect;
	public ParticleSystem[] muzzleEffect;
	public ParticleSystem bulletCapsuleEffect;
	public ParticleSystem hitEffectHole;
	public ParticleSystem hitEffect;
	public ParticleSystem bloodEffect;

	// Default values
	[ShowNonSerializedField] private float muzzleVel = 900f;
	[ShowNonSerializedField] private float C = 0.295f;
	[ShowNonSerializedField] private float p = 1.225f;
	[ShowNonSerializedField] private float A = 0.000048f;
	[ShowNonSerializedField] private float m = 0.016f;
	[ShowNonSerializedField] private float gravity = 9.81f;

	// Weapon values
	[ReadOnly] public int ammoMagazine;
	[ShowNonSerializedField] private bool isFiring;
	[ShowNonSerializedField] private bool isReloading;

	[Header("Shooting")]
	public Transform raycastOrigin;
	public Transform raycastDestination;

	List<Bullet> bullets = new List<Bullet>();

	Ray ray;
	RaycastHit hitInfo;

	private void Awake()
	{
		muzzleVel = weaponPreset.shootVelocity;
		C = weaponPreset.bulletCoefficient;
		p = (float)worldPhysics.densidadDelAire;
		A = Mathf.PI * Mathf.Pow(weaponPreset.diameter * 0.0005f, 2f);
		m = weaponPreset.weight * 0.001f;
		gravity = worldPhysics.gravedad;

		ammoMagazine = weaponPreset.magazineCapacity;
		isFiring = false;
		isReloading = false;
	}

	Bullet CreateBullet(Vector3 position, Vector3 velocity)
	{
		Bullet bullet = new Bullet();
		bullet.time = 0;
		bullet.position = position;
		bullet.velocity = velocity;
		bullet.tracerEffect = Instantiate(tracerEffect, position, Quaternion.identity);
		bullet.tracerEffect.AddPosition(position);
		return bullet;
	}

	public void StartFiring()
	{
		if (!isFiring && !isReloading)
			if (ammoMagazine > 0)
				if (weaponPreset.burst)
					if (weaponPreset.shootDelay == 0)
						StartCoroutine(Shotgun());
					else
						StartCoroutine(Burst());
				else
					StartCoroutine(Shoot());
			else
				StartCoroutine(Reload());
	}

	IEnumerator Shoot()
	{
		isFiring = true;
		// Effects
		foreach (var particle in muzzleEffect)
			particle.Emit(1);
		bulletCapsuleEffect.Emit(1);
		// Actual shoot
		FireBullet();
		yield return new WaitForSeconds(weaponPreset.shootDelay);
		isFiring = false;
	}

	IEnumerator Burst()
	{
		isFiring = true;
		// Actual shoot
		for (int i = 0; i < weaponPreset.burstQuantity; i++)
		{
			// Effects
			foreach (var particle in muzzleEffect)
				particle.Emit(1);
			bulletCapsuleEffect.Emit(1);
			FireBullet();
			yield return new WaitForSeconds(weaponPreset.shootDelay);
			if (ammoMagazine <= 0)
				break;
		}
		yield return new WaitForSeconds(weaponPreset.burstDelay);
		isFiring = false;
	}

	IEnumerator Shotgun()
	{
		isFiring = true;
		// Effects
		foreach (var particle in muzzleEffect)
			particle.Emit(1);
		bulletCapsuleEffect.Emit(1);
		// Actual shoot
		for (int i = 0; i < weaponPreset.burstQuantity; i++)
		{
			FireBullet();
			yield return new WaitForSeconds(weaponPreset.shootDelay);
			if (ammoMagazine <= 0)
				break;
		}
		yield return new WaitForSeconds(weaponPreset.burstDelay);
		isFiring = false;
	}

	void FireBullet()
	{
		ammoMagazine--;

		var maxDispersion = weaponPreset.dispersion;

		Vector3 dispersion = new Vector3(Random.Range(-maxDispersion, maxDispersion), Random.Range(-maxDispersion, maxDispersion));
		Vector3 velocity = ((raycastDestination.position - raycastOrigin.position).normalized + dispersion) * muzzleVel;
		bullets.Add(CreateBullet(raycastOrigin.position, velocity));

		if (ammoMagazine <= 0)
			StartCoroutine(Reload());
	}

	public IEnumerator Reload()
	{
		isReloading = true;
		yield return new WaitForSeconds(weaponPreset.reloadDelay);
		ammoMagazine = weaponPreset.magazineCapacity;
		isReloading = false;
		reload.Invoke();
	}

	Vector3 GetPosition(Bullet bullet)
	{
		var v = bullet.velocity.magnitude;
		var t = bullet.time;
		
		var dir = bullet.velocity.normalized;

		var f = v * t - (C * p * A * v * v * t * t) / 2f * m;
		var g = Vector3.down * .5f * gravity * t * t;

		return bullet.position + dir * f + g;
	}

	void FixedUpdate()
	{
		SimulateBullets();
		DestroyBullets();
	}

	void SimulateBullets()
	{
		bullets.ForEach(bullet =>
		{
			Vector3 p0 = GetPosition(bullet);
			bullet.time += Time.fixedDeltaTime;
			Vector3 p1 = GetPosition(bullet);
			RaycastSegment(p0, p1, bullet);
		});
	}

	void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
	{
		Vector3 direction = end - start;
		float distance = direction.magnitude;
		ray.origin = start;
		ray.direction = direction;
		if (Physics.Raycast(ray, out hitInfo, distance))
		{
			//Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.red, 1f);

			bullet.tracerEffect.transform.position = hitInfo.point;
			bullet.time = 2f;

			if (hitInfo.collider.gameObject.TryGetComponent(out HpManager hp))
			{
				hp.ReceiveDamage(weaponPreset.damage);
				bloodEffect.transform.position = hitInfo.point;
				bloodEffect.transform.forward = hitInfo.normal;
				bloodEffect.Emit(20);
			}
			else
			{
				if (hitInfo.collider.gameObject.TryGetComponent(out Rigidbody rb))
				{
					rb.AddForce(ray.direction.normalized * 10, ForceMode.Impulse);
					hitEffect.transform.position = hitInfo.point;
					hitEffect.transform.forward = hitInfo.normal;
					hitEffect.Emit(1);
				}
				else
				{
					hitEffectHole.transform.position = hitInfo.point;
					hitEffectHole.transform.forward = hitInfo.normal;
					hitEffectHole.Emit(1);
				}
			}
		}
		else
			bullet.tracerEffect.transform.position = end;
	}

	void DestroyBullets() {
		bullets.RemoveAll(bullet => bullet.time >= 2f);
	}
}
