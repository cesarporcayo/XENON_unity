using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;

	  public float speed;
    public float maxSpeed;

    public AudioSource jumpSound;
    public AudioSource hookSound;

    public float jumpPower;
    private bool grounded;

  	public float turnSpeed = 0.25f;

  	private bool nearShop = false;

    public GameObject cameraman;
    public GameObject costume;
    public float costumeSwingFactor = 0.9f;
		private GameObject camera;

    private float turretPlaceTimer = 0f;

    [SerializeField]
    public Transform turretPrefab;


    void Start() {
    	rb = GetComponent<Rigidbody>();
			camera = GameObject.FindWithTag("MainCamera");
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


      if (Input.GetButtonDown("Jump")) {
          if (grounded) {
              grounded = false;
              rb.AddForce(Vector3.up * jumpPower);
              jumpSound.Play();
          }
      }

      if (Input.GetButtonDown("Build") && turretPlaceTimer <= 0f) {
        turretPlaceTimer = 1;
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
      } else {
        turretPlaceTimer -= Time.deltaTime;
      }

      if (GetComponent<Rigidbody>().velocity.magnitude > maxSpeed) {
         GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * maxSpeed;
      }
    }

    private void OnCollisionEnter(Collision collision) {
      foreach (ContactPoint contact in collision.contacts) {
        if (contact.point.y < transform.position.y) {
          grounded = true;
        }
      }
    }

    public void closeToShop() {
        nearShop = true;
    }

    public void notCloseToShop() {
        nearShop = false;
    }

    public void shootHook() {
        hookSound.Play();
    }
}
