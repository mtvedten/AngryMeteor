using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDamage : MonoBehaviour {

	public int hitPoints = 2;
	public Sprite damageSprite;
	public float damageSpeed;

	private int currentHitPoints;
	private float damageImpactSpeedSqr;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		currentHitPoints = hitPoints;
		damageImpactSpeedSqr = damageSpeed * damageSpeed;
	}
	
	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.collider.tag != "Damager") {
			return;
		}
		if (collision.relativeVelocity.sqrMagnitude < damageImpactSpeedSqr) {
			return;
		}

		spriteRenderer.sprite = damageSprite;
		currentHitPoints--;

		if (currentHitPoints <= 0) {
			Kill();
		}

	}


	void Kill(){
		spriteRenderer.enabled = false;
		GetComponent<Collider2D>().enabled = false;
		GetComponent<Rigidbody2D>().isKinematic = true;
		GetComponent<ParticleSystem>().Play();
	}
	
}
