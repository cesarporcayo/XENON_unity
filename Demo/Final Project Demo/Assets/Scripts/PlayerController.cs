﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;

	  public float speed;
    public float maxSpeed;

    public float jumpPower;
    private bool grounded;
    //public int grounded = 0;

  	public GameObject bullet;
  	public Transform bulletSpawn;

  	public float fireRate;
  	private float nextFire;
  	public float turnSpeed = 0.25f;

  	private bool nearShop = false;

    public GameObject cameraman;
    public GameObject costume;
    public float costumeSwingFactor = 0.9f;
		private GameObject camera;

    [SerializeField]
    public Transform turretPrefab;


    void Start() {
    	rb = GetComponent<Rigidbody>();
			camera = GameObject.FindWithTag("MainCamera");
    }

    void update() {
    	if (Input.GetButton("Fire1") && Time.time > nextFire) {
    		nextFire = Time.time + fireRate;
    		Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
    	}
    }

		Quaternion turretRot = Quaternion.identity;

		Ray ray;
		RaycastHit hit;

    void FixedUpdate() {
    	float moveHorizontal = Input.GetAxis("Horizontal");
    	float moveVertical = Input.GetAxis("Vertical");

    	Vector3 movement =  cameraman.transform.rotation * new Vector3 (moveHorizontal, 0.0f, moveVertical);

    	rb.AddForce(movement * speed);


        costume.transform.position = Vector3.Lerp(costume.transform.position, transform.position, costumeSwingFactor);


        if (Input.GetKeyDown(KeyCode.Space)) {
            // Debug.Log("space");
            if (grounded) {
                grounded = false;
                rb.AddForce(Vector3.up * jumpPower);
                // Debug.Log("jump!");
            }  
            //else if (grounded == 1 && doubleJump) {
            //    grounded = 2;
            //    rb.AddForce(Vector3.up * jumpPower);
            //}
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            // Debug.Log("spawn turret!");
						turretRot = Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0);
						ray = new Ray(transform.position + turretRot * Vector3.forward * 2f, -1 * Vector3.up);
						if (Physics.Raycast(ray, out hit)) {
							if (hit.transform.gameObject.name == "Terrain") {
								if (CurrencyScript.buyTurret()) {
		            	Object.Instantiate(turretPrefab, hit.point + 0.5f * Vector3.up, turretRot);
								}
							}
						}
        }

        if (GetComponent<Rigidbody>().velocity.magnitude > maxSpeed)
        {
           GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * maxSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
		foreach (ContactPoint contact in collision.contacts) {
			if (contact.point.y < transform.position.y) {
                grounded = true;
			}
      }
    }

    public void closeToShop()
    {
        nearShop = true;
    }

    public void notCloseToShop()
    {
        nearShop = false;
    }

    // void updateShoot() {
    // 	Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
    // }

    //   void updateHook() {
    //   	if (Input.GetKeyDown("z")) {

    // }
    //   }

}
