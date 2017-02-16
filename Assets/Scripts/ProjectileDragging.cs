using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDragging : MonoBehaviour {

	public float maxStrech = 3.0f;
	public LineRenderer catapultLineFront;
	public LineRenderer catapultLineBack;

	private SpringJoint2D spring;
	private Transform catapult;
	private float maxStrechSqr;
	private Ray rayToMouse;
	private Ray leftCatapultToProjectile;
	//private float maxStrechSqr;
	private float circleRadius;
	private bool clickedOn; 
	private Vector2 prevVelocity;
//private Rigidbody2D rigidbody2D = GetC Rigidbody2D();



	void Awake() {
		spring = GetComponent<SpringJoint2D>();
		//Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
		catapult = spring.connectedBody.transform;
	}
	// Use this for initialization
	void Start () {
		LineRendererSetup();
		rayToMouse = new Ray(catapult.position, Vector3.zero);
		leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);
		maxStrechSqr = maxStrech * maxStrech;
		CircleCollider2D circle = GetComponent<CircleCollider2D>(); 
		circleRadius = circle.radius;
	}
	
	// Update is called once per frame
	void Update () {


		if(clickedOn) {
			Dragging();
		}

		if (spring != null) {
			if (!GetComponent<Rigidbody2D>().isKinematic && prevVelocity.sqrMagnitude > GetComponent<Rigidbody2D>().velocity.sqrMagnitude) {
				Destroy(spring);
				GetComponent<Rigidbody2D>().velocity = prevVelocity;
			}

			if (!clickedOn) {
				prevVelocity = GetComponent<Rigidbody2D>().velocity;
			}

			LineRenderUpdate(); 
			

		} else {
			catapultLineFront.enabled = false;
			catapultLineBack.enabled = false;
		}
	}

	void LineRendererSetup () {
		catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
		catapultLineBack.SetPosition(0, catapultLineBack.transform.position);

		catapultLineFront.sortingLayerName = "Foreground";
		catapultLineBack.sortingLayerName = "Foreground";

		catapultLineFront.sortingOrder = 3;
		catapultLineBack.sortingOrder = 1;
			
	}

	void OnMouseDown () {
		Debug.Log("Mouse down");
		spring.enabled = false;
		clickedOn = true;
	}

	void OnMouseUp () {
		Debug.Log("Mouse up");
		spring.enabled = true;
		GetComponent<Rigidbody2D>().isKinematic = false;
		clickedOn = false;
	}

	void Dragging() {
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - catapultLineBack.transform.position; 

		if (catapultToMouse.sqrMagnitude > maxStrechSqr) {
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint(maxStrech);
		}
		
		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	}

	void LineRenderUpdate() {
		Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
		leftCatapultToProjectile.direction = catapultToProjectile;
		Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);
		catapultLineFront.SetPosition(1, holdPoint);
		catapultLineBack.SetPosition(1, holdPoint);
	}
}
